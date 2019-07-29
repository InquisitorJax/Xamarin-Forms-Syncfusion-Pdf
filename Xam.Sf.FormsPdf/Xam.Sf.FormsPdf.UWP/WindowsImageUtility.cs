using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Shapes;
using Size = SixLabors.Primitives.Size;
using System.IO;

namespace Xam.Sf.FormsPdf.UWP
{
	public class WindowsImageUtility : IImageUtility
	{
		public byte[] TransformIntoCircle(byte[] image)
		{
			//doc: https://github.com/SixLabors/Samples/blob/master/ImageSharp/AvatarWithRoundedCorner/Program.cs
			byte[] retImageBytes;
			using (var img = Image.Load(image))
			{
				var breadth = img.Width > img.Height ? img.Height : img.Width;
				var cornerRadius = breadth / 2;
				var size = new Size(breadth, breadth);
				using (var cropped = img.Clone(x => WindowsImageUtility.ConvertToAvatar(x, size, cornerRadius)))
				{
					var memoryStream = new MemoryStream();
					cropped.SaveAsPng(memoryStream);
					retImageBytes = memoryStream.ToArray();
				}

			}

			return retImageBytes;
		}

		private static IImageProcessingContext<Rgba32> ConvertToAvatar(IImageProcessingContext<Rgba32> processingContext, Size size, float cornerRadius)
		{
			return processingContext.Resize(new ResizeOptions
			{
				Size = size,
				Mode = ResizeMode.Crop
			}).Apply(i => ApplyRoundedCorners(i, cornerRadius));
		}

		public static void ApplyRoundedCorners(Image<Rgba32> img, float cornerRadius)
		{
			IPathCollection corners = BuildCorners(img.Width, img.Height, cornerRadius);

			var graphicOptions = new GraphicsOptions(true)
			{
				AlphaCompositionMode = PixelAlphaCompositionMode.DestOut // enforces that any part of this shape that has color is punched out of the background
			};
			// mutating in here as we already have a cloned original
			// use any color (not Transparent), so the corners will be clipped
			img.Mutate(x => x.Fill(graphicOptions, Rgba32.LimeGreen, corners));
		}

		public static IPathCollection BuildCorners(int imageWidth, int imageHeight, float cornerRadius)
		{
			// first create a square
			var rect = new RectangularPolygon(-0.5f, -0.5f, cornerRadius, cornerRadius);

			// then cut out of the square a circle so we are left with a corner
			IPath cornerTopLeft = rect.Clip(new EllipsePolygon(cornerRadius - 0.5f, cornerRadius - 0.5f, cornerRadius));

			// corner is now a corner shape positions top left
			//lets make 3 more positioned correctly, we can do that by translating the orgional artound the center of the image

			float rightPos = imageWidth - cornerTopLeft.Bounds.Width + 1;
			float bottomPos = imageHeight - cornerTopLeft.Bounds.Height + 1;

			// move it across the width of the image - the width of the shape
			IPath cornerTopRight = cornerTopLeft.RotateDegree(90).Translate(rightPos, 0);
			IPath cornerBottomLeft = cornerTopLeft.RotateDegree(-90).Translate(0, bottomPos);
			IPath cornerBottomRight = cornerTopLeft.RotateDegree(180).Translate(rightPos, bottomPos);

			return new PathCollection(cornerTopLeft, cornerBottomLeft, cornerTopRight, cornerBottomRight);
		}
	}

}

