using Android.Content;
using Android.Graphics;
using Android.OS;
using Java.IO;

namespace Xamarin.Core.Droid
{
	public static class FileUtil
	{
		public static File CreateTempFile(Context context, string fileName, string fileExtension)
		{
			var directory = context.GetExternalFilesDir(Environment.DirectoryPictures);
			try
			{
				return File.CreateTempFile(fileName, fileExtension, directory);
			}
			catch (System.Exception e)
			{
				System.Diagnostics.Debug.Write(e.StackTrace);
				return null;
			}
		}

		public static File CreateTempFileFromBitmap(Context context, Bitmap bitmap, string fileName, string fileExtension, int compressedQuality = 50)
		{
			var directory = context.GetExternalFilesDir(Environment.DirectoryPictures);
			try
			{
				var file = File.CreateTempFile(fileName, fileExtension, directory);
				var fileStream = new System.IO.FileStream(file.Path, System.IO.FileMode.OpenOrCreate);
				bitmap.Compress(Bitmap.CompressFormat.Jpeg, compressedQuality, fileStream);
				fileStream.Flush();
				fileStream.Close();
				return file;
			}
			catch (System.Exception e)
			{
				System.Diagnostics.Debug.Write(e.StackTrace);
				return null;
			}
		}

		public static bool Exists(string filePath)
		{
			var file = new File(filePath);
			return file.Exists();
		}
	}
}
