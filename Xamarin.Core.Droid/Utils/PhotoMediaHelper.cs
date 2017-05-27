using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Net;
using Android.Widget;
using Uri = Android.Net.Uri;

namespace Xamarin.Core.Droid
{
	public class PhotoMediaHelper : CameraHelper.OnPhotoCapturedListener, GalleryHelper.OnPhotoSelectedListener
	{
		private readonly Activity _activity;
		private readonly CameraHelper _cameraHelper;
		private readonly GalleryHelper _galleryHelper;

		private ImageView _ivPhoto;
		private AlertDialog _popupDialog;
		private string _tag;

		private OnPhotoEditedListener _listener;

		public PhotoMediaHelper(Activity activity) : this(activity, null, null)
		{
			_cameraHelper = new CameraHelper(activity);
			_galleryHelper = new GalleryHelper(activity);
		}

		public PhotoMediaHelper(Activity activity, CameraHelper cameraUtil, GalleryHelper galleryUtil)
		{
			_activity = activity;
			_cameraHelper = cameraUtil;
			_galleryHelper = galleryUtil;
		}

		public void ShowPhotoEditMenu(ImageView view, string tag, OnPhotoEditedListener listener)
		{
			_ivPhoto = view;
			_tag = tag;
			_listener = listener;

			if (_popupDialog != null && _popupDialog.IsShowing)
			{
				_popupDialog.Dismiss();
			}

			_popupDialog = CreatePopupDialog(_activity);
			_popupDialog.Show();
		}

		public void OpenCamera(CameraHelper.OnPhotoCapturedListener listener)
		{
			if (_popupDialog != null && _popupDialog.IsShowing)
			{
				_popupDialog.Dismiss();
			}

			if (_cameraHelper != null)
			{
				_cameraHelper.OpenCamera(listener);
			}
		}

		protected void OpenCamera()
		{
			OpenCamera(this);
		}

		public void OpenGalley(GalleryHelper.OnPhotoSelectedListener listener)
		{
			if (_popupDialog != null && _popupDialog.IsShowing)
			{
				_popupDialog.Dismiss();
			}

			if (_galleryHelper != null)
			{
				_galleryHelper.OpenGallery(listener);
			}
		}

		protected void OpenGalley()
		{
			OpenGalley(this);
		}

		public async Task HandleOnActivityResult(int requestCode, int resultCode, Intent data)
		{
			if (_cameraHelper != null)
			{
				await _cameraHelper.HandleActivityResult(requestCode, resultCode, data);
			}
			if (_galleryHelper != null)
			{
				await _galleryHelper.HandleActivityResult(requestCode, resultCode, data);
			}
		}

		protected virtual AlertDialog CreatePopupDialog(Activity activity)
		{
			string[] menuArray = { "Take photo", "Select from gallery", "Cancel" };

			var builder = new AlertDialog.Builder(activity);
			builder.SetItems(menuArray, (sender, e) =>
			{
				switch (e.Which)
				{
					case 0:
						OpenCamera();
						break;
					case 1:
						OpenGalley();
						break;
					default:
						_popupDialog.Dismiss();
						_listener = null;
						break;
				}
			});

			return builder.Create();
		}

		private AlertDialog CreatePopupDialog(Activity activity, string[] menuArray, System.EventHandler<DialogClickEventArgs> e)
		{
			var builder = new AlertDialog.Builder(activity);
			builder.SetItems(menuArray, e);
			return builder.Create();
		}

		public void OnPhotoCaptured(Bitmap bitmap, Uri uri)
		{
			EditPhoto(bitmap, uri);
		}

		public void OnPhotoSelected(Bitmap bitmap, Uri uri)
		{
			EditPhoto(bitmap, uri);
		}

		private void EditPhoto(Bitmap photoBitmap, Uri imageUri)
		{
			if (_listener != null)
			{
				_listener.OnPhotoEdited(_ivPhoto, photoBitmap, imageUri, _tag);
				_listener = null;
			}
		}

		public interface OnPhotoEditedListener
		{
			void OnPhotoEdited(ImageView view, Bitmap bitmap, Uri uri, string tag);
		}
	}
}
