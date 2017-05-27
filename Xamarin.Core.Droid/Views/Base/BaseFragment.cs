using System;
using System.Collections.Generic;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;

using GalaSoft.MvvmLight.Helpers;
using Microsoft.Practices.ServiceLocation;
using Xamarin.Core;
using Xamarin.Core.ViewModels;

namespace Contemi.Droid.Views
{
    public abstract class BaseFragment<TViewModel> : BaseFragment where TViewModel : BaseViewModel
    {
        public override INavigator Navigator
        {
            get { return base.Navigator; }
            set
            {
                base.Navigator = value;
                ViewModel.Navigator = value;
            }
        }

        protected TViewModel _viewModel;

        /// <summary>
        /// First version: read-only ViewModel
        /// Editable ViewModel will be implemented later
        /// </summary>
        /// <value>The view model.</value>
        public TViewModel ViewModel
        {
            get
            {
                if (_viewModel == null)
                {
                    SetViewModel(ServiceLocator.Current.GetInstance<TViewModel>());
                }

                return _viewModel;
            }

            set
            {
                SetViewModel(value);
            }
        }

        private void SetViewModel(TViewModel viewModel)
        {
            if (viewModel != _viewModel)
            {
                //RemoveBindings();
                _viewModel = viewModel;
                if (_viewModel != null)
                {
                    _viewModel.Navigator = this.Navigator;
                }
                //SetBindingsIfNeed();
            }
        }

        public override void OnDetach()
        {
            base.OnDetach();

            ViewModel?.Cleanup();
            ViewModel = null;
        }

        public override void OnPause()
        {
            base.OnPause();
            RemoveBindings();
        }

        public override void OnResume()
        {
            SetBindingsIfNeed();
            base.OnResume();
        }

        protected abstract void SetBindings();

        protected readonly List<Binding> _bindings = new List<Binding>();

        protected virtual void RemoveBindings()
        {
            _bindings.ForEach(x => x?.Detach());
            _bindings.Clear();
            _isBound = false;
            System.Diagnostics.Debug.WriteLine($">>> Remove bindings...{Key}");
        }

        private bool _isBound = false;
        private readonly object _locker = new object();
        private void SetBindingsIfNeed()
        {
            if (!_isBound)
            {
                lock (_locker)
                {
                    if (!_isBound)
                    {
                        System.Diagnostics.Debug.WriteLine($">>> Set bindings...{Key}");
                        SetBindings();
                        _isBound = true;
                    }
                }
            }
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel.RegisterMessages();
        }

        public override void OnDestroy()
        {
            ViewModel.UnregisterMessages();

            base.OnDestroy();
        }
    }

    public abstract class BaseFragment : Fragment, IScreen, Animator.IAnimatorListener // , View.IOnClickListener
    {
        protected abstract int LayoutResource { get; }

        protected View RootView { get; set; }

        protected LayoutInflater Inflater { get; set; }

        public virtual INavigator Navigator { get; set; }

        public object NavigationParameter { get; set; }

        public int Key { get; set; }

        public bool IsRoot { get; set; }

        public bool NoCache { get; set; }

        public AnimationType AnimationType { get; set; } = AnimationType.Fade;

        protected virtual bool TranslucentStatusBar
        {
            get { return false; }
        }

        public Action AnimationEnded { get; set; }

        public bool IsNavigateBack { get; set; }

        protected View FindViewById(int id)
        {
            return RootView?.FindViewById(id);
        }

        protected TView FindViewById<TView>(int id) where TView : View
        {
            if (RootView == null)
            {
                return default(TView);
            }
            return RootView.FindViewById<TView>(id);
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        //public override void OnDestroyView()
        //{
        //    base.OnDestroyView();
        //    if (RootView != null && RootView.Parent != null)
        //    {
        //        var parent = RootView.Parent as ViewGroup;
        //        parent.RemoveView(RootView);
        //    }
        //}

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (RootView == null)
            {
                Inflater = inflater; 
                RootView = inflater.Inflate(LayoutResource, null, false);
                // TODO: use IAnimatorViewGroup in fragment's root layout to reduce a level of view hierarchy
                if (!(RootView is IAnimatorViewGroup))
                {
                    var animLayout = new AnimatorLinearLayout(this.Activity)
                    {
                        Orientation = Orientation.Vertical
                    };
                    animLayout.AddView(RootView, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
                    RootView = animLayout;
                }

                OnCreatedView(inflater);

                OnCreatedViewWithBundle(savedInstanceState);

                RegisterEventsIfNeed();
            }
            else if (RootView.Parent != null)
            {
                var parent = RootView.Parent as ViewGroup;
                parent.RemoveView(RootView);
            }

            return RootView;
        }

        protected virtual void OnNavigateBack()
        {
        }

        protected abstract void OnCreatedView(LayoutInflater inflater);

        protected virtual void OnCreatedViewWithBundle(Bundle savedInstanceState)
        {
        }

        protected virtual void RegisterEvents() { }

        /// <summary>
        /// Manually unregister events 
        /// (thinking about how to unregister events automatically)
        /// </summary>
        protected virtual void UnregisterEvents() { }

        private bool _registeredEvents = false;
        private void RegisterEventsIfNeed()
        {
            if (!_registeredEvents)
            {
                RegisterEvents();
                _registeredEvents = true;
            }
        }

        public override void OnDetach()
        {
            UnregisterEvents();
            base.OnDetach();
        }

        private bool _isLoaded = false;

        public override void OnResume()
        {
            base.OnResume();
            if (!IsNavigateBack && !_isLoaded)
            {
                LoadData(NavigationParameter);
                _isLoaded = true;
            }
            else
            {
                OnNavigateBack();
            }

            HandleStatusBar();
        }

        private void HandleStatusBar()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                if (TranslucentStatusBar)
                {
                    Activity.Window.AddFlags(WindowManagerFlags.TranslucentStatus);
                }
                else
                {
                    Activity.Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
                }
            }
        }

        protected virtual void LoadData(object arg) { }

        public override void OnPause()
        {
            HideKeyboard();

            base.OnPause();
        }

        protected virtual void ResetControls()
        {
        }

        protected virtual void HideKeyboard()
        {
            if (RootView != null)
            {
                var imm = (InputMethodManager)Application.Context.GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(RootView.WindowToken, 0);
            }
        }

        public override Animator OnCreateAnimator(FragmentTransit transit, bool enter, int nextAnim)
        {
            var animator = TransitionExtensions.LoadAnimator(AnimationType, transit, enter);
            animator?.AddListener(this);
            return animator;
        }

        #region Animator listener

        public virtual void OnAnimationCancel(Animator animation)
        {
        }

        public virtual void OnAnimationEnd(Animator animation)
        {
            AnimationEnded?.Invoke();
            AnimationEnded = null;
        }

        public virtual void OnAnimationRepeat(Animator animation)
        {
        }

        public virtual void OnAnimationStart(Animator animation)
        {
        }

		public void Log(string message)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}

