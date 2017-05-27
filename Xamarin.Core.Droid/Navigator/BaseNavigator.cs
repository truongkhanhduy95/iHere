using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Android.Content;
using Android.Views.InputMethods;
using Microsoft.Practices.ServiceLocation;
using Xamarin.Core;
using Xamarin.Core.Droid;
using Android.App;


namespace Xamarin.Core.Droid
{
	public class Navigator : INavigator
	{
		private readonly Dictionary<int, Type> _screenSpecs = new Dictionary<int, Type>();

		private Stack<Tuple<int, Fragment>> _screenStack;
		private Activity _hostActivity;
		private FragmentManager _fragmentManager;

		public int ContainerLayoutId { get; set; }

		public INavigationConfig Configuration
		{
			get
			{
				// Shouldn't use singleton
				return ServiceLocator.Current.GetInstance<INavigationConfig>();
			}
		}

		public int CurrentPageKey
		{
			get
			{
				return _screenStack.Peek().Item1;
			}
		}

		/// <summary>
		/// Caution: check how to get current fragment
		/// </summary>
		/// <value>The current screen.</value>
		public IScreen CurrentScreen
		{
			get
			{
				var manager = _fragmentManager;
				//return manager.FindFragmentById(ContainerLayoutId) as IScreen;
				if (manager.BackStackEntryCount > 0)
				{
					var tag = manager.GetBackStackEntryAt(manager.BackStackEntryCount - 1).Name;
					var fragment = manager.FindFragmentByTag(tag);
					return fragment as IScreen;
				}
				// if not exist
				return null;
			}
		}

		public void Initialize(Activity activity, FragmentManager fragmentManager, int containerLayoutId)
		{
			_hostActivity = activity;
			_fragmentManager = fragmentManager;
			if (_screenStack != null)
			{
				_screenStack.Clear();
			}
			else
			{
				_screenStack = new Stack<Tuple<int, Fragment>>();
			}
			_screenStack.Push(new Tuple<int, Fragment>(-1, null));

			ContainerLayoutId = containerLayoutId;
			Configuration.Setup(this);
		}

		public void GoBack()
		{
			if (_fragmentManager.BackStackEntryCount > 1)
			{
				var tag = _fragmentManager.GetBackStackEntryAt(_fragmentManager.BackStackEntryCount - 1).Name;
				var fragment = _fragmentManager.FindFragmentByTag(tag);
				SetIsNavigateBack(fragment, true);
				_fragmentManager.PopBackStack();
				_screenStack.Pop();
				SystemBackHandler.Instance.PopFromBackStack();

				var lastScreen = _screenStack.Peek();
				if (lastScreen != null)
				{
					_fragmentManager.BeginTransaction()
									.Add(ContainerLayoutId, lastScreen.Item2, lastScreen.Item1.ToString())
									.CommitAllowingStateLoss();
				}
			}
		}

		public void HideKeyboard()
		{
			var inputManager = (InputMethodManager)_hostActivity.GetSystemService(Context.InputMethodService);
			// check if no view has focus:

			if (_hostActivity.CurrentFocus != null)
			{
				inputManager.HideSoftInputFromWindow(_hostActivity.CurrentFocus.WindowToken, 0);
			}
		}

		public void GoBack(int pageKey)
		{
			lock (_screenSpecs)
			{
				if (!_screenSpecs.ContainsKey(pageKey))
				{
					throw new ArgumentException($"You dont go to this controller: {pageKey}. so you cannot back to it!!", nameof(pageKey));
				}
				if (_screenStack.Peek().Item1 == pageKey)
				{
					return;
				}

				Type type = _screenSpecs[pageKey];
				for (int i = _fragmentManager.BackStackEntryCount - 1; i >= 0; i--)
				{
					var tag = _fragmentManager.GetBackStackEntryAt(i).Name;
					var fragment = _fragmentManager.FindFragmentByTag(tag);
					if (tag == pageKey.ToString())
					{
						SetIsNavigateBack(fragment, true);

						var lastScreen = _screenStack.Peek();
						if (lastScreen != null)
						{
							_fragmentManager.BeginTransaction()
											.Add(ContainerLayoutId, lastScreen.Item2, lastScreen.Item1.ToString())
											.CommitAllowingStateLoss();
						}
						return;
					}
					_fragmentManager.PopBackStack();
					_screenStack.Pop();
					SystemBackHandler.Instance.PopFromBackStack();
				}

				NavigateTo(pageKey);
			}
		}

		private void SetIsNavigateBack(Fragment fragment, bool isBack)
		{
			if (fragment is IScreen)
			{
				(fragment as IScreen).IsNavigateBack = isBack;
			}
		}

		public void NavigateTo(int pageKey)
		{
			NavigateTo(pageKey, null);
		}

		public void NavigateTo(int pageKey, object parameter)
		{
			if (_hostActivity == null)
			{
				throw new InvalidOperationException("No CurrentActivity found");
			}

			lock (_screenSpecs)
			{
				_hostActivity.RunOnUiThread(() =>
				{
					System.Diagnostics.Debug.WriteLine(string.Format("From screen: {1}, To Screen: {0}", pageKey, CurrentPageKey));

					if (CurrentPageKey == pageKey)
					{
						if (CurrentScreen != null)
						{
							CurrentScreen.NavigationParameter = parameter;
							(CurrentScreen as Fragment)?.OnResume();
						}
						return;
					}

					var destinationFragment = InitFragment(pageKey, parameter);
					if (destinationFragment == null)
					{
						throw new InvalidOperationException("Can't Init Fragment");
					}

					var toScreen = destinationFragment as IScreen;
					toScreen.IsNavigateBack = false;
					if (toScreen.IsRoot)
					{
						// Clear backstack
						Invalidate();
						System.Diagnostics.Debug.WriteLine($">>>>>>>>>>>>>>>> Current screen's key >>> {CurrentScreen?.Key}");
					}

                    var fragment = destinationFragment as BaseFragment;
                    var currentFragment = CurrentScreen as Fragment;
                    if (fragment != null)
                    {
                        fragment.AnimationEnded = new Action(() =>
                        {
                            if (currentFragment != null)
                            {
                                _fragmentManager.BeginTransaction()
                                                .Remove(currentFragment)
                                                .CommitAllowingStateLoss();
                            }
                        });
                    }

                    var transaction = _fragmentManager.BeginTransaction()
													  .Add(ContainerLayoutId, destinationFragment, pageKey.ToString())
													  .SetTransition(FragmentTransit.FragmentOpen);

					if ((CurrentScreen == null || !CurrentScreen.NoCache) && !toScreen.IsRoot)
					{
						// Add to backstack if cache is enable
						transaction = transaction.AddToBackStack(pageKey.ToString());
					}

					transaction.CommitAllowingStateLoss();
					HideKeyboard();
					_screenStack.Push(new Tuple<int, Fragment>(pageKey, destinationFragment));
					SystemBackHandler.Instance.PushToBackStack(toScreen);
				});
			}
		}

		public void Invalidate()
		{
			for (int i = _fragmentManager.BackStackEntryCount - 1; i >= 0; i--)
			{
				var tag = _fragmentManager.GetBackStackEntryAt(i).Name;
				var fragment = _fragmentManager.FindFragmentByTag(tag);
				_fragmentManager.PopBackStackImmediate();
				_screenStack.Pop();
			}
		}

		public void Configure(int key, Type activityType)
		{
			lock (_screenSpecs)
			{
				if (_screenSpecs.ContainsKey(key))
				{
					_screenSpecs[key] = activityType;
				}
				else
				{
					_screenSpecs.Add(key, activityType);
				}
			}
		}

		protected Fragment InitFragment(int screenkey, object parameter)
		{
			if (!_screenSpecs.ContainsKey(screenkey))
			{
				throw new Exception($"Dev >>> Not contain screen with key {screenkey}");
			}

			ConstructorInfo constructor;
			object[] parameters;
			Type fragmentType = _screenSpecs[screenkey];
			if (parameter == null)
			{
				constructor = fragmentType.GetTypeInfo()
					.DeclaredConstructors
					.FirstOrDefault(c => !c.GetParameters().Any());

				parameters = new object[]
				{
				};
			}
			else
			{
				constructor = fragmentType.GetTypeInfo()
					.DeclaredConstructors
					.FirstOrDefault(
					c =>
					{
						var p = c.GetParameters();
						return p.Count() == 1
						&& p[0].ParameterType == parameter.GetType();
					});

				if (constructor != null)
				{
					parameters = new[]
					{
						parameter
					};
				}
				else
				{
					constructor = fragmentType.GetConstructor(new Type[0]);
					parameters = new object[]
					{
					};
				}
			}

			var fragment = constructor.Invoke(parameters);
			var screen = fragment as IScreen;
			if (screen != null)
			{
				screen.Navigator = this;
				screen.NavigationParameter = parameter;
				screen.Key = screenkey;
				Configuration.Decorate(screen);
			}

			return fragment as Fragment;
		}

		public bool ContainsKey(int key)
		{
			return _screenSpecs.ContainsKey(key);
		}

		public void Present(int pageKey, Action onDismissed = null)
		{
			throw new NotImplementedException();
		}

		public void Present(int pageKey, object parameter, Action onDismissed = null)
		{
			throw new NotImplementedException();
		}

		public void Dismiss()
		{
			throw new NotImplementedException();
		}

		public void NavigateExternal(string dataText)
		{
			//if (!string.IsNullOrEmpty(dataText))
			//{
			//	dataText = dataText.Replace(" ", "");
			//	double phoneNumber = 0;

			//	string action = Intent.ActionView;

			//	if (dataText.Contains("@"))
			//	{
			//		dataText = string.Format("mailto:{0}", dataText);
			//		action = Intent.ActionView;
			//	}
			//	else if (double.TryParse(dataText, out phoneNumber))
			//	{
			//		dataText = string.Format("tel:{0}", dataText);
			//		action = Intent.ActionDial;
			//	}
			//	else if (!dataText.Contains("http://") && !dataText.Contains("https://"))
			//	{
			//		dataText = "http://" + dataText;
			//	}

			//	var browserIntent = new Intent(action, Android.Net.Uri.Parse(dataText));
			//	_hostActivity.StartActivity(browserIntent);
			//}
		}
	}
}
