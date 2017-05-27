
using System;
using Android.Animation;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Microsoft.Practices.ServiceLocation;
using Xamarin.Core.Interfaces.Views;

namespace Xamarin.Core.Droid
{
    public class LoadingOverlayView : FrameLayout, ILoading
    {
        private LoadingView _loadingView;
        private ProgressBar _progressBar;
        private AppCompatTextView _titleTextView;
        private Handler _handler;

        private LoadingView LoadingView
        {
            get
            {
                return _loadingView ?? (_loadingView = FindViewById<LoadingView>(Resource.Id.loadingView));
            }
        }

        private ProgressBar ProgressBar
        {
            get
            {
                return _progressBar ?? (_progressBar = FindViewById<ProgressBar>(Resource.Id.loadingViewLollipop));
            }
        }

        private AppCompatTextView TitleTextView
        {
            get
            {
                return _titleTextView ?? (_titleTextView = FindViewById<AppCompatTextView>(Resource.Id.loadingTitleTextView));
            }
        }

        public string Title
        {
            get { return TitleTextView?.Text; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    TitleTextView.Visibility = ViewStates.Gone;
                }
                else
                {
                    TitleTextView.Visibility = ViewStates.Visible;
                    TitleTextView.Text = value;
                }
            }
        }

        public LoadingOverlayView() :
            base(Application.Context)
        {
            Initialize();
        }

        private void Initialize()
        {
            var layoutParams = new LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            layoutParams.Gravity = GravityFlags.Center;

            var view = Inflate(Context, Resource.Layout.LoadingOverlayView, null);
            view.LayoutParameters = layoutParams;
            AddView(view);

            LoadingView.Color = ResourcesUtil.GetColor(Context, Resource.Color.google_blue);
            TitleTextView.SetTextColor(LoadingView.Color);
            SetBackgroundColor(Color.Argb(0, 0, 0, 0));

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                LoadingView.Visibility = ViewStates.Gone;
            }
            else
            {
                ProgressBar.Visibility = ViewStates.Gone;
            }

            _handler = new Handler(Looper.MainLooper);
        }

        private void ExecuteOnMainThread(Action action)
        {
            if (Handler == null)
            {
                _handler.Post(action);
            }
            else
            {
                Handler.Post(action);
            }
        }

        public void Show(Action onCompleted = null)
        {
            ExecuteOnMainThread(() =>
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    ProgressBar.Visibility = ViewStates.Visible;
                }
                else
                {
                    LoadingView.Running = true;
                    LoadingView.Visibility = ViewStates.Visible;
                    var fadeAnimator = ObjectAnimator.OfFloat(this, "alpha", 0, 1);
                    fadeAnimator.SetDuration(200);
                    fadeAnimator.AnimationEnd += (sender, e) => onCompleted?.Invoke();
                    fadeAnimator.Start();

                    Clickable = true;
                }
            });
        }

        public void Hide(Action onCompleted = null)
        {
            ExecuteOnMainThread(() =>
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    ProgressBar.Visibility = ViewStates.Gone;
                }
                else
                {
                    LoadingView.Visibility = ViewStates.Gone;
                    var fadeAnimator = ObjectAnimator.OfFloat(this, "alpha", 1, 0);
                    fadeAnimator.SetDuration(200);
                    fadeAnimator.AnimationEnd += (sender, e) =>
                    {
                        onCompleted?.Invoke();
                        LoadingView.Running = false;
                        Clickable = false;
                    };
                    fadeAnimator.Start();
                }
                Title = string.Empty;
            });
        }
    }
}

