using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.Database;
using Android.Graphics;
using Android.Media;
using Android.Net;
using Android.Provider;
using Java.Lang;

namespace Xamarin.Core.Droid
{
	public static class ImageUtil
	{
		public static Bitmap RotateImage(Bitmap src, int degree)
		{
			if (degree > 0)
			{
				var matrix = new Matrix();
				matrix.PostRotate(degree);

				src = Bitmap.CreateBitmap(src, 0, 0, src.Width, src.Height, matrix, true);
			}
			return src;
		}

		public static int GetOrientation(Context context, Uri photoUri)
		{
			int orientation = GetOrientationFromExif(photoUri.Path);
			if (orientation <= 0)
			{
				orientation = GetOrientationFromMediaStore(context, photoUri);
			}

			return orientation;
		}

		public static async Task<Bitmap> DecodePhotoAsync(Context context, Uri photoUri, int maxSize)
		{
			var stream = context.ContentResolver.OpenInputStream(photoUri);
			var options = new BitmapFactory.Options();
			options.InJustDecodeBounds = true;
			await BitmapFactory.DecodeStreamAsync(stream, null, options);
			stream.Close();

			int rotatedWidth, rotatedHeight;
			int orientation = GetOrientation(context, photoUri);

			if (orientation == 90 || orientation == 270)
			{
				rotatedWidth = options.OutHeight;
				rotatedHeight = options.OutWidth;
			}
			else
			{
				rotatedWidth = options.OutWidth;
				rotatedHeight = options.OutHeight;
			}

			Bitmap src;
			stream = context.ContentResolver.OpenInputStream(photoUri);
			if (rotatedWidth > maxSize || rotatedHeight > maxSize)
			{
				float widthRatio = (float)rotatedWidth / (float)maxSize;
				float heightRatio = (float)rotatedHeight / (float)maxSize;
				float maxRatio = Math.Max(widthRatio, heightRatio);

				options.InSampleSize = (int)maxRatio;
				options.InJustDecodeBounds = false;
				src = await BitmapFactory.DecodeStreamAsync(stream, null, options);
			}
			else
			{
				src = await BitmapFactory.DecodeStreamAsync(stream);
			}
			stream.Close();
			return src;
		}

		public static Bitmap DecodePhoto(Context context, Uri photoUri, int maxSize)
		{
			var options = new BitmapFactory.Options();

			int rotatedWidth, rotatedHeight;
			int orientation = GetOrientation(context, photoUri);

			if (orientation == 90 || orientation == 270)
			{
				rotatedWidth = options.OutHeight;
				rotatedHeight = options.OutWidth;
			}
			else
			{
				rotatedWidth = options.OutWidth;
				rotatedHeight = options.OutHeight;
			}

			Bitmap src;
			var stream = context.ContentResolver.OpenInputStream(photoUri);
			if (rotatedWidth > maxSize || rotatedHeight > maxSize)
			{
				float widthRatio = (float)rotatedWidth / (float)maxSize;
				float heightRatio = (float)rotatedHeight / (float)maxSize;
				float maxRatio = Math.Max(widthRatio, heightRatio);

				options.InSampleSize = (int)maxRatio;
				options.InJustDecodeBounds = false;
				src = BitmapFactory.DecodeStream(stream, null, options);
			}
			else
			{
				src = BitmapFactory.DecodeStream(stream);
			}
			stream.Close();
			return src;
		}

		public static string ConvertBitmapToBase64(Bitmap bitmap)
		{
			using (var stream = new MemoryStream())
			{
				bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
				byte[] byteArray = stream.ToArray();
				return System.Convert.ToBase64String(byteArray);
			}
		}

		public static string ConvertUriToBase64(Context context, string uri)
		{
			var bitmap = MediaStore.Images.Media.GetBitmap(context.ContentResolver, Uri.Parse(uri));
			return ConvertBitmapToBase64(bitmap);
		}

		private static int GetOrientationFromExif(string imagePath)
		{
			int orientation = -1;
			try
			{
				var exif = new ExifInterface(imagePath);
				int exifOrientation = exif.GetAttributeInt(ExifInterface.TagOrientation, (int)Orientation.Normal);

				switch (exifOrientation)
				{
					case (int)Orientation.Rotate270:
						orientation = 270;
						break;
					case (int)Orientation.Rotate180:
						orientation = 180;
						break;
					case (int)Orientation.Rotate90:
						orientation = 90;
						break;
					case (int)Orientation.Normal:
						orientation = 0;
						break;
				}
			}
			catch (Java.IO.IOException e)
			{
				e.PrintStackTrace();
			}
			return orientation;
		}

		private static int GetOrientationFromMediaStore(Context context, Uri photoUri)
		{
			if (photoUri == null)
			{
				return -1;
			}

			string[] projection = { MediaStore.Images.ImageColumns.Orientation };
			ICursor cursor = context.ContentResolver.Query(photoUri, projection, null, null, null);

			int orientation = -1;
			if (cursor != null && cursor.MoveToFirst())
			{
				orientation = cursor.GetInt(0);
				cursor.Close();
			}
			return orientation;
		}
	}
}
