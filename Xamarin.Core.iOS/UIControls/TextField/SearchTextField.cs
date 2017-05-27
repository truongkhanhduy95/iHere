using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Xamarin.Core.iOS
{
	[Register("SearchTextField")]
	public class SearchTextField : XamarinTextField, ISearchInputField, IUITableViewDelegate
	{
		private const int DefaultSearchDelay = 1000;

		private bool _isSearching;
		private Timer _searchTimer;
		private ElapsedEventHandler _timeElapsed;

		private UIView _tableContainerView;
		protected UITextView _lblEmpty;
		protected UITableView _tableView;
		protected BaseTableViewDataSource<ISearchResultItem> _dataSource;
		private UITapGestureRecognizer _tapOutsideGesture;

		public event EventHandler<PerformSearchEventArgs> PerformSearch;
		public event EventHandler OnCancelSearch;
		public event EventHandler<SearchResultItemClickEventArgs> ItemClick;

		protected virtual int MaxRowVisibility { get { return 3; } }

		protected virtual int RowHeight { get { return 44; } }

		public int SearchDelay { get; set; }

		public List<ISearchResultItem> SearchResultList { get; private set; }

		public SearchTextField(IntPtr handle) : base(handle) { Initialize(); }

		public SearchTextField() : base() { Initialize(); }

		public SearchTextField(CGRect frame) : base(frame) { Initialize(); }

		private void Initialize()
		{
			SearchDelay = DefaultSearchDelay;
			_isSearching = false;

			_timeElapsed = HandleElapsedEventHandler;
			_tapOutsideGesture = new UITapGestureRecognizer(TapOutside);
			_tapOutsideGesture.CancelsTouchesInView = false;
			_tapOutsideGesture.DelaysTouchesEnded = false;

			_tableView = new UITableView(Frame, UITableViewStyle.Plain);
			_tableView.WeakDelegate = this;
			_tableView.ScrollEnabled = true;
			_tableView.UserInteractionEnabled = true;
			_tableView.Bounces = false;
			_tableView.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine;
			_tableView.SeparatorColor = UIColor.LightGray;
			_tableView.SeparatorInset = UIEdgeInsets.Zero;
			_tableView.BackgroundColor = UIColor.White;
			_tableView.Layer.BorderWidth = 1;
			_tableView.Layer.BorderColor = UIColor.LightGray.CGColor;

			_lblEmpty = new UITextView();
			_lblEmpty.Font = UIFont.PreferredBody;
			_lblEmpty.TextColor = UIColor.LightGray;
			_lblEmpty.TextAlignment = UITextAlignment.Center;
			_lblEmpty.ContentMode = UIViewContentMode.Center;
			_lblEmpty.BackgroundColor = UIColor.White;
			_lblEmpty.Layer.BorderWidth = 1;
			_lblEmpty.Layer.BorderColor = UIColor.LightGray.CGColor;
			_lblEmpty.Text = "No Results Found.";

			_tableContainerView = new UIView(new CGRect(CGPoint.Empty, UIScreen.MainScreen.Bounds.Size));
			_tableContainerView.AddSubview(_tableView);
			_tableContainerView.AddSubview(_lblEmpty);

			AddTarget(OnEditingChanged, UIControlEvent.EditingChanged);
		}

		public void SetSearchResult(List<ISearchResultItem> results)
		{
			if (IsFirstResponder)
			{
				SearchResultList = results;
				InvokeOnMainThread(ShowSearchResults);
			}
		}

		public override bool BecomeFirstResponder()
		{
			CancelSearch();
			return base.BecomeFirstResponder();
		}

		public override bool ResignFirstResponder()
		{
			CancelSearch();
			return base.ResignFirstResponder();
		}

		private void StartSearchTimer()
		{
			CancelSearch();

			_isSearching = true;
			_searchTimer = new Timer();
			_searchTimer.Interval = SearchDelay;
			_searchTimer.Elapsed += _timeElapsed;
			_searchTimer.Start();
		}

		private void CancelSearch()
		{
			_isSearching = false;

			if (_searchTimer != null)
			{
				_searchTimer.Elapsed -= _timeElapsed;
				_searchTimer.Stop();
				_searchTimer.Dispose();
				_searchTimer = null;
			}

			if (_tableContainerView != null && _tableContainerView.Superview != null)
			{
				if (_tableContainerView.GestureRecognizers.Contains(_tapOutsideGesture))
				{
					_tableContainerView.RemoveGestureRecognizer(_tapOutsideGesture);
				}
				_tableContainerView.RemoveFromSuperview();
			}
		}

		private void HandleElapsedEventHandler(object sender, ElapsedEventArgs e)
		{
			_searchTimer.Stop();
			if (PerformSearch != null)
			{
				InvokeOnMainThread(StartSearching);
			}
		}

		private void OnEditingChanged(object sender, EventArgs e)
		{
			CancelSearch();
			if (IsFirstResponder && !_isSearching && !string.IsNullOrEmpty(Text))
			{
				StartSearchTimer();
			}
		}

		protected virtual void StartSearching()
		{
			var args = new PerformSearchEventArgs(Text);
			PerformSearch.Invoke(this, args);
		}

		protected virtual void ShowSearchResults()
		{
			CancelSearch();

			UIView parentView = this;
			CGPoint point = Frame.Location;
			do
			{
				parentView = parentView.Superview;
				if (parentView.Frame.Location.X < Window.Bounds.Width)
				{
					point.X += parentView.Frame.Location.X;
				}
				point.Y += parentView.Frame.Location.Y;
				if (parentView is UIScrollView)
				{
					point.Y -= ((UIScrollView)parentView).ContentOffset.Y;
				}
			} while (parentView.Superview != null);
			var frame = new CGRect(point, Frame.Size);
			frame.Y += Frame.Size.Height;

			if (SearchResultList != null && SearchResultList.Count > 0)
			{
				_tableView.Hidden = false;
				_lblEmpty.Hidden = true;

				if (SearchResultList.Count > MaxRowVisibility)
				{
					frame.Height = RowHeight * MaxRowVisibility + (RowHeight / 4);
				}
				else
				{
					frame.Height = RowHeight * SearchResultList.Count;
				}

				if (frame.Y + frame.Height >= Window.Bounds.Height / 2)
				{
					frame.Y -= frame.Height + Frame.Height;
				}

				_tableView.Frame = frame;

				if (_dataSource == null)
				{
					_dataSource = CreateDataSource(SearchResultList);
					_dataSource.ItemClicked += OnItemClick;
					_tableView.Source = _dataSource;
				}
				else
				{
					_dataSource.DataSource.Clear();
					_dataSource.DataSource.AddRange(SearchResultList);
				}

				_tableView.ReloadData();
			}
			else
			{
				_tableView.Hidden = true;
				_lblEmpty.Hidden = false;

				frame.Height = RowHeight;

				if (frame.Y + frame.Height >= Window.Bounds.Height / 2)
				{
					frame.Y -= frame.Height + Frame.Height;
				}

				_lblEmpty.Frame = frame;
			}

			parentView.AddSubview(_tableContainerView);
			_tableContainerView.AddGestureRecognizer(_tapOutsideGesture);
		}

		private void OnItemClick(object sender, ItemClickEventArgs<ISearchResultItem> e)
		{
			ResignFirstResponder();

			if (SearchResultList != null && SearchResultList.Count > 0 && SearchResultList.Count <= (e.Position - 1))
			{
				var resultItem = SearchResultList[e.Position];
				if (resultItem != null)
				{
					Text = SearchResultList[e.Position].SearchResultText;

					if (ItemClick != null)
					{
						var args = new SearchResultItemClickEventArgs(resultItem, e.Position);
						ItemClick.Invoke(this, args);
					}
				}
			}
		}

		private void TapOutside(UITapGestureRecognizer gestureRecognizer)
		{
			var tapLocation = gestureRecognizer.LocationInView(_tableContainerView);
			var tapView = gestureRecognizer.View.HitTest(tapLocation, null);
			if (tapView == _tableContainerView)
				ResignFirstResponder();
		}

		protected virtual BaseTableViewDataSource<ISearchResultItem> CreateDataSource(List<ISearchResultItem> resultItemList)
		{
			return null;
		}
	}
}
