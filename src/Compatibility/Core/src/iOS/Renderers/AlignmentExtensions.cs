using ObjCRuntime;
using UIKit;

namespace Microsoft.Maui.Controls.Compatibility.Platform.iOS
{
	[PortHandler]
	internal static class AlignmentExtensions
	{
		internal static UITextAlignment ToPlatformTextAlignment(this TextAlignment alignment, EffectiveFlowDirection flowDirection)
		{
			var isLtr = flowDirection.IsLeftToRight();
			switch (alignment)
			{
				case TextAlignment.Center:
					return UITextAlignment.Center;
				case TextAlignment.End:
					if (isLtr)
						return UITextAlignment.Right;
					else
						return UITextAlignment.Left;
				default:
					if (isLtr)
						return UITextAlignment.Left;
					else
						return UITextAlignment.Right;
			}
		}

		internal static UIControlContentVerticalAlignment ToPlatformTextAlignment(this TextAlignment alignment)
		{
			switch (alignment)
			{
				case TextAlignment.Center:
					return UIControlContentVerticalAlignment.Center;
				case TextAlignment.End:
					return UIControlContentVerticalAlignment.Bottom;
				case TextAlignment.Start:
					return UIControlContentVerticalAlignment.Top;
				default:
					return UIControlContentVerticalAlignment.Top;
			}
		}
	}
}