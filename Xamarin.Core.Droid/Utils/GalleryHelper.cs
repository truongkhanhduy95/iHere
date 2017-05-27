using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Net;
using Android.Provider;
using Java.IO;

namespace Xamarin.Core.Droid
{
	public class GalleryHelper
	{
		private const int RequestGallerySelect = 102;
		private const int DefaultMaxImageSize = 800;

		private readonly Activity _activity;

		private OnPhotoSelectedListener _listener;

		public int MaxSize { get; set; }

		public GalleryHelper(Activity activity)
		{
			_activity = activity;
			MaxSize = DefaultMaxImageSize;
		}

		public void OpenGallery(OnPhotoSelectedListener listener)
		{
			var intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
			intent.SetType("image/*");
			intent.SetAction(Intent.ActionGetContent);
			_activity.StartActivityForResult(Intent.CreateChooser(intent, "GALLERY"), RequestGallerySelect);

			_listener = listener;
		}

		public async Task HandleActivityResult(int requestCode, int resultCode, Intent data)
		{
			if (requestCode == RequestGallerySelect && resultCode == (int)Result.Ok)
			{
				try
				{
					if (_listener != null)
					{
						Uri uri = null;
						if (data != null && data.Data != null)
						{
							uri = data.Data;
						}
						int orientation = ImageUtil.GetOrientation(_activity, uri);

						Bitmap decodedBitmap = await ImageUtil.DecodePhotoAsync(_activity, uri, MaxSize);
						Bitmap resultBitmap = ImageUtil.RotateImage(decodedBitmap, orientation);

						_listener.OnPhotoSelected(resultBitmap, uri);
						_listener = null;
					}
				}
				catch (IOException e)
				{
					e.PrintStackTrace();
				}
			}
		}

		public interface OnPhotoSelectedListener
		{
			void OnPhotoSelected(Bitmap bitmap, Uri uri);
		}
	}
}
