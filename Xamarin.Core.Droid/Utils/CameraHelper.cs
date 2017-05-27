using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Net;
using Android.OS;
using Android.Provider;
using Java.IO;

namespace Xamarin.Core.Droid
{
	public class CameraHelper
	{
		private int DefaultMaxImageSize = 800;
		private const int RequestCameraCapture = 101;

		private readonly Activity _activity;
		private string _currentPhotoPath = "";

		private OnPhotoCapturedListener _listener;

		public int MaxSize { get; set; }

		public CameraHelper(Activity activity)
		{
			_activity = activity;
			MaxSize = DefaultMaxImageSize;
		}

		private File GetDirectoryForPictures(Context context)
		{
			var applicationInfo = context.PackageManager.GetApplicationInfo(context.PackageName, PackageInfoFlags.Activities);
			var applicationName = context.PackageManager.GetApplicationLabel(applicationInfo);

			var dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), applicationName);
			if (!dir.Exists())
			{
				dir.Mkdirs();
			}
			return dir;
		}

		public void OpenCamera(OnPhotoCapturedListener listener)
		{
			var intent = new Intent(MediaStore.ActionImageCapture);
			if (intent.ResolveActivity(_activity.PackageManager) != null)
			{
				string fileName = string.Format("Photo_{0}.jpg", System.Guid.NewGuid());
				var photoFile = new File(GetDirectoryForPictures(_activity), fileName);
				_currentPhotoPath = "file:" + photoFile.AbsolutePath;

				intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(photoFile));
				_activity.StartActivityForResult(intent, RequestCameraCapture);

				_listener = listener;
			}
		}

		public async Task HandleActivityResult(int requestCode, int resultCode, Intent data)
		{
			if (requestCode == RequestCameraCapture && resultCode == (int)Result.Ok)
			{
				try
				{
					if (_listener != null)
					{
						Uri uri = Uri.Parse(_currentPhotoPath);
						int orientation = ImageUtil.GetOrientation(_activity, uri);

						var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
						mediaScanIntent.SetData(uri);
						_activity.SendBroadcast(mediaScanIntent);

						Bitmap decodedBitmap = await ImageUtil.DecodePhotoAsync(_activity, uri, MaxSize);
						Bitmap resultBitmap = ImageUtil.RotateImage(decodedBitmap, orientation);

						_listener.OnPhotoCaptured(resultBitmap, uri);
						_listener = null;
					}
				}
				catch (IOException e)
				{
					e.PrintStackTrace();
				}
			}
		}

		public interface OnPhotoCapturedListener
		{
			void OnPhotoCaptured(Bitmap bitmap, Uri uri);
		}
	}
}
