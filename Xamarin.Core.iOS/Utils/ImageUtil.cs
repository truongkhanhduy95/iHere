using System;
using System.IO;
using System.Runtime.InteropServices;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Xamarin.Core.iOS
{
	public static class ImageUtil
	{
		public static string ConvertImageToBase64(UIImage image)
		{
			byte[] byteArray = null;
			using (var imageData = image.AsPNG())
			{
				byteArray = new byte[imageData.Length];
				Marshal.Copy(imageData.Bytes, byteArray, 0, Convert.ToInt32(imageData.Length));
			}
			return Convert.ToBase64String(byteArray);
		}

		public static string SaveImageToApplicationDirectory(UIImage image)
		{
			var documentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var imageName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
			var imagePath = Path.Combine(documentDirectory, imageName);
			var imageData = image.AsJPEG();
			NSError error;
			if (imageData.Save(imagePath, false, out error))
			{
				return imagePath;
			}
			return null;
		}

		public static UIImage ScaleAndRotateImage(UIImage image, int maxSize)
		{
			UIImage res;

			using (CGImage imageRef = image.CGImage)
			{
				CGImageAlphaInfo alphaInfo = imageRef.AlphaInfo;
				CGColorSpace colorSpaceInfo = CGColorSpace.CreateDeviceRGB();
				if (alphaInfo == CGImageAlphaInfo.None)
				{
					alphaInfo = CGImageAlphaInfo.NoneSkipLast;
				}

				nint width, height;

				width = imageRef.Width;
				height = imageRef.Height;

				if (maxSize > 0)
				{
					if (height >= width)
					{
						width = (int)Math.Floor((double)width * ((double)maxSize / (double)height));
						height = maxSize;
					}
					else
					{
						height = (int)Math.Floor((double)height * ((double)maxSize / (double)width));
						width = maxSize;
					}
				}

				CGBitmapContext bitmap;

				if (image.Orientation == UIImageOrientation.Up || image.Orientation == UIImageOrientation.Down)
				{
					bitmap = new CGBitmapContext(IntPtr.Zero, width, height, imageRef.BitsPerComponent, imageRef.BytesPerRow, colorSpaceInfo, alphaInfo);
				}
				else
				{
					bitmap = new CGBitmapContext(IntPtr.Zero, height, width, imageRef.BitsPerComponent, imageRef.BytesPerRow, colorSpaceInfo, alphaInfo);
				}

				switch (image.Orientation)
				{
					case UIImageOrientation.Left:
						bitmap.RotateCTM((float)Math.PI / 2);
						bitmap.TranslateCTM(0, -height);
						break;
					case UIImageOrientation.Right:
						bitmap.RotateCTM(-((float)Math.PI / 2));
						bitmap.TranslateCTM(-width, 0);
						break;
					case UIImageOrientation.Up:
						break;
					case UIImageOrientation.Down:
						bitmap.TranslateCTM(width, height);
						bitmap.RotateCTM(-(float)Math.PI);
						break;
				}

				bitmap.DrawImage(new CGRect(0, 0, width, height), imageRef);

				res = UIImage.FromImage(bitmap.ToImage());
				bitmap = null;
			}

			return res;
		}
	}
}
