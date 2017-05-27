using System;
using System.Collections.Generic;
using System.Timers;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using R = Android.Resource;

namespace Xamarin.Core.Droid
{
	public class SearchEditText : XamarinEditText, ISearchInputField, AdapterView.IOnItemSelectedListener
	{
		private const int DefaultSearchDelay = 1000;

		private bool _isSearching;
		private Timer _searchTimer;
		private ElapsedEventHandler _timeElapsed;

		private ListPopupWindow _popupWindow;

		public int SearchDelay { get; set; }

		public List<ISearchResultItem> SearchResultList { get; private set; }

		protected virtual string EmptyMessage { get { return "No Results Found."; } }

		public event EventHandler<PerformSearchEventArgs> PerformSearch;
		public event EventHandler OnCancelSearch;
		public event EventHandler<SearchResultItemClickEventArgs> ItemClick;

		protected SearchEditText(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { Initialize(); }

		public SearchEditText(Context context) :
					base(context)
		{
			Initialize();
		}

		public SearchEditText(Context context, IAttributeSet attrs) :
					base(context, attrs)
		{
			Initialize();
		}

		public SearchEditText(Context context, IAttributeSet attrs, int defStyle) :
					base(context, attrs, defStyle)
		{
			Initialize();
		}

		private void Initialize()
		{
			SearchDelay = DefaultSearchDelay;
			_isSearching = false;
			_timeElapsed = HandleElapsedEventHandler;

			AfterTextChanged += SearchEditTextAfterTextChanged;
		}

		public void SetSearchResult(List<ISearchResultItem> results)
		{
			if (IsFocused)
			{
				SearchResultList = results;
				((Activity)Context).RunOnUiThread(ShowResults);
			}
		}

		protected override void OnTextChanged(Java.Lang.ICharSequence text, int start, int lengthBefore, int lengthAfter)
		{
			base.OnTextChanged(text, start, lengthBefore, lengthAfter);
			CancelSearch();
		}

		private void SearchEditTextAfterTextChanged(object sender, AfterTextChangedEventArgs e)
		{
			if (IsFocused && !_isSearching && !string.IsNullOrEmpty(Text))
			{
				StartSearchTimer();
			}
		}

		protected virtual void HandleElapsedEventHandler(object sender, ElapsedEventArgs e)
		{
			_searchTimer.Stop();

			if (PerformSearch != null)
			{
				var args = new PerformSearchEventArgs(Text);
				PerformSearch.Invoke(this, args);
			}
		}

		private void CancelSearch()
		{
			_isSearching = false;

			if (_searchTimer != null)
			{
				_searchTimer.Stop();
				_searchTimer.Dispose();
				_searchTimer = null;
			}

			if (_popupWindow != null)
			{
				_popupWindow.SetOnItemSelectedListener(null);
			}

			if (OnCancelSearch != null)
			{
				OnCancelSearch.Invoke(this, new EventArgs());
			}
		}

		private void StartSearchTimer()
		{
			if (_popupWindow == null)
			{
				_popupWindow = new ListPopupWindow(Context);
				_popupWindow.AnchorView = this;
			}

			if (_popupWindow.IsShowing)
			{
				_popupWindow.Dismiss();
			}

			_isSearching = true;
			_searchTimer = new Timer();
			_searchTimer.Interval = SearchDelay;
			_searchTimer.Elapsed += _timeElapsed;
			_searchTimer.Start();
		}

		protected virtual void ShowResults()
		{
			CancelSearch();

			var dropDownSource = new List<string>();
			if (SearchResultList != null && SearchResultList.Count > 0)
			{
				foreach (var item in SearchResultList)
				{
					if (!string.IsNullOrEmpty(item.SearchResultText))
					{
						dropDownSource.Add(item.SearchResultText);
					}
				}
			}
			else
			{
				dropDownSource.Add(EmptyMessage);
			}

			var adapter = new ArrayAdapter<string>(Context, R.Layout.SimpleDropDownItem1Line, R.Id.Text1, dropDownSource);
			_popupWindow.SetAdapter(adapter);
			_popupWindow.SetOnItemSelectedListener(this);
			_popupWindow.Show();
		}

		public void OnItemSelected(AdapterView parent, View view, int position, long id)
		{
			ClearFocus();
			SoftKeyboardUtil.HideKeyboard(Context, this);

			if (_popupWindow != null)
			{
				_popupWindow.Dismiss();
				_popupWindow.SetOnItemSelectedListener(null);
			}

			if (SearchResultList != null && SearchResultList.Count > 0 && SearchResultList.Count <= (position - 1))
			{
				var resultItem = SearchResultList[position];
				if (resultItem != null)
				{
					Text = SearchResultList[position].SearchResultText;

					if (ItemClick != null)
					{
						var args = new SearchResultItemClickEventArgs(resultItem, position);
						ItemClick.Invoke(this, args);
					}
				}
			}
		}

		public void OnNothingSelected(AdapterView parent)
		{
			ClearFocus();
			SoftKeyboardUtil.HideKeyboard(Context, this);

			if (_popupWindow != null)
			{
				_popupWindow.Dismiss();
				_popupWindow.SetOnItemSelectedListener(null);
			}
		}
	}
}
