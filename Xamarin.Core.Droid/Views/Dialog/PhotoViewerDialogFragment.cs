using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using R = Android.Resource;
using Com.Davemorrissey.Labs.Subscaleview;

namespace Xamarin.Core.Droid
{
	public class PhotoViewerDialogFragment : DialogFragment
	{
		private const string KeyViewType = "KeyViewType";
		private const string KeyImagePath = "KeyImagePath";
		private const string KeyImageBitmap = "KeyImageBitmap";

		private const string ViewTypeUri = "ViewTypeUri";
		private const string ViewTypeBitmap = "ViewTypeBitmap";

		private SubsamplingScaleImageView _ivPhoto;
		private string _viewType;

		public static PhotoViewerDialogFragment CreateFragment(string imagePath)
		{
			var dialogFragment = new PhotoViewerDialogFragment();
			var args = new Bundle();
			args.PutString(KeyImagePath, imagePath);
			args.PutString(KeyViewType, ViewTypeUri);
			dialogFragment.Arguments = args;
			return dialogFragment;
		}

		public static PhotoViewerDialogFragment CreateFragment(Bitmap imageBitmap)
		{
			var dialogFragment = new PhotoViewerDialogFragment();
			var args = new Bundle();
			args.PutParcelable(KeyImageBitmap, imageBitmap);
			args.PutString(KeyViewType, ViewTypeBitmap);
			dialogFragment.Arguments = args;
			return dialogFragment;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var frameLayout = new FrameLayout(Context);
			frameLayout.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);

			_ivPhoto = new SubsamplingScaleImageView(Context);
			_ivPhoto.LayoutParameters = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
			_ivPhoto.MaxScale = 3f;
			frameLayout.AddView(_ivPhoto);

			var gestureDetector = new GestureDetector(Context, new DismissDialogGesture(this, _ivPhoto));
			_ivPhoto.SetOnTouchListener(new DismissDialogListener(gestureDetector));
			_ivPhoto.Orientation = SubsamplingScaleImageView.OrientationUseExif;

			_viewType = Arguments.GetString(KeyViewType, ViewTypeUri);
			switch (_viewType)
			{
				case ViewTypeUri:
					string url = Arguments.GetString(KeyImagePath, "");
					_ivPhoto.SetImage(ImageSource.InvokeUri(url));
					break;
				case ViewTypeBitmap:
					var bitmap = Arguments.GetParcelable(KeyImageBitmap) as Bitmap;
					_ivPhoto.SetImage(ImageSource.InvokeBitmap(bitmap));
					break;
			}

			Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

			return frameLayout;
		}

		public override void OnResume()
		{
			base.OnResume();
			Dialog.Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
			Dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
			Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
		}

		public override void OnDismiss(IDialogInterface dialog)
		{
			base.OnDismiss(dialog);
			switch (_viewType)
			{
				case ViewTypeUri:
					_ivPhoto.SetImage(ImageSource.InvokeResource(R.Color.Transparent));
					break;
				case ViewTypeBitmap:
					break;
			}
		}

		private class DismissDialogGesture : GestureDetector.SimpleOnGestureListener
		{
			private readonly DialogFragment _dialog;
			private readonly SubsamplingScaleImageView _ivImagView;

			public DismissDialogGesture(DialogFragment dialog, SubsamplingScaleImageView ivImagView) : base()
			{
				_dialog = dialog;
				_ivImagView = ivImagView;
			}

			public override bool OnSingleTapConfirmed(MotionEvent e)
			{
				if (_ivImagView.IsReady && _dialog != null)
				{
					_dialog.Dismiss();
				}
				return true;
			}
		}

		private class DismissDialogListener : Java.Lang.Object, View.IOnTouchListener
		{
			private readonly GestureDetector _gesture;

			public DismissDialogListener(GestureDetector gesture)
			{
				_gesture = gesture;
			}

			public bool OnTouch(View v, MotionEvent e)
			{
				if (_gesture != null)
				{
					return _gesture.OnTouchEvent(e);
				}
				return false;
			}
		}
	}
}
