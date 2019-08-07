using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Xam.Sf.FormsPdf.iOS
{
	public class AppleImageUtility : IImageUtility
	{
		public byte[] TransformIntoCircle(byte[] image)
		{
			UIImage uiImage = new UIImage(NSData.FromArray(image));
			uiImage = CropSquare(uiImage);
			uiImage = CropCircle(uiImage);

			var result = uiImage.AsPNG().ToArray();

			return result;
		}

		private UIImage CropCircle(UIImage image)
		{
			// stretches the image if it's not square 
			var newSize = image.Size;
			var minEdge = Math.Min(newSize.Height, newSize.Width);

			var size = new CGSize(width: minEdge, height: minEdge);

			UIGraphics.BeginImageContextWithOptions(size, false, 0.0f);
			var context = UIGraphics.GetCurrentContext();

			image.Draw(new CGRect(CGPoint.Empty, size), CGBlendMode.Copy, alpha: 1.0f);

			context.SetBlendMode(CGBlendMode.Copy);
			context.SetFillColor(UIColor.Clear.CGColor);

			var rect = new CGRect(CGPoint.Empty, size: size);
			var rectPath = UIBezierPath.FromRect(rect);
			var circlePath = UIBezierPath.FromOval(new CGRect(CGPoint.Empty, size: size));

			rectPath.AppendPath(circlePath);
			rectPath.UsesEvenOddFillRule = true;
			rectPath.Fill();

			var result = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();

			return result;
		}

		private UIImage CropToCircle(UIImage image)
		{
			//once square, could use this to add a border as well

			nfloat size = image.Size.Height <= image.Size.Width ? image.Size.Height : image.Size.Width;
			// only works for square images
			UIImageView imageView = new UIImageView(image)
			{
				ContentMode = UIViewContentMode.ScaleAspectFit
			};

			var layer = imageView.Layer;
			layer.MasksToBounds = true;
			layer.CornerRadius = size / 2;
			//layer.BorderColor = UIColor.Orange.CGColor;
			//layer.BorderWidth = 4;
			UIGraphics.BeginImageContext(imageView.Bounds.Size);
			layer.RenderInContext(UIGraphics.GetCurrentContext());
			var roundedImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			return roundedImage;
		}



		public UIImage CropImage(UIImage image, CGRect cropRect)
		{
			UIGraphics.BeginImageContextWithOptions(cropRect.Size, false, 0);
			var context = UIGraphics.GetCurrentContext();

			context.TranslateCTM(0.0f, image.Size.Height);
			context.ScaleCTM(1.0f, -1.0f);
			context.DrawImage(new CGRect(0, 0, image.Size.Width, image.Size.Height), image.CGImage);
			context.ClipToRect(cropRect);

			var croppedImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();

			return croppedImage;
		}

		private UIImage CropSquare(UIImage image)
		{
			CGSize contextSize = image.Size;
			nfloat posX = 0.0f;
			nfloat posY = 0.0f;
			nfloat width = image.Size.Width;
			nfloat height = image.Size.Height;

			if (image.Size.Width > image.Size.Height)
			{
				posX = (image.Size.Width - image.Size.Height) / 2;
				posY = 0;

				width = image.Size.Height;
				height = image.Size.Height;

			}
			else
			{
				posX = 0;
				posY = (image.Size.Height - image.Size.Width) / 2;
				width = contextSize.Width;
				height = contextSize.Width;
			}

			CGRect rect = new CGRect(posX, posY, width, height);

			var retImage = CropImage(image, rect);

			return retImage;
		}
	}
}