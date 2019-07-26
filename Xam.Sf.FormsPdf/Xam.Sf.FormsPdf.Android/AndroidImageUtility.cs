using System.IO;
using Android.Graphics;
using static Android.Graphics.Bitmap;

namespace Xam.Sf.FormsPdf.Droid
{
	public class AndroidImageUtility : IImageUtility
	{
		public byte[] TransformIntoCircle(byte[] image)
		{
			Bitmap originalImage = BitmapFactory.DecodeByteArray(image, 0, image.Length);

			var circleImage = GetRoundedCroppedBitmap(originalImage);

			var croppedImage = AndroidImageUtility.ToByteArray(circleImage, 100);

			return croppedImage;
		}

		public static byte[] ToByteArray(Bitmap image, int quality)
		{
			byte[] retImage = null;
			using (MemoryStream outStream = new MemoryStream())
			{
				Bitmap.CompressFormat compressFormat = Bitmap.CompressFormat.Png;
				image.Compress(compressFormat, quality, outStream);
				retImage = outStream.ToArray();
			};
			return retImage;
		}

		private Bitmap GetRoundedCroppedBitmap(Bitmap bitmap)
		{
			int widthLight = bitmap.Width;
			int heightLight = bitmap.Height;

			Bitmap output = Bitmap.CreateBitmap(bitmap.Width, bitmap.Height, Config.Argb8888);

			Canvas canvas = new Canvas(output);
			Paint paintColor = new Paint();
			paintColor.Flags = PaintFlags.AntiAlias;

			RectF rectF = new RectF(new Rect(0, 0, widthLight, heightLight));

			canvas.DrawRoundRect(rectF, widthLight / 2, heightLight / 2, paintColor);

			Paint paintImage = new Paint();
			paintImage.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcAtop));
			canvas.DrawBitmap(bitmap, 0, 0, paintImage);

			return output;
		}

	}
}