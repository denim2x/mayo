using System;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

namespace Microsoft.Maui.Controls.Compatibility.Platform.iOS
{
	[System.Obsolete]
	public static class VisualElementExtensions
	{
		public static IVisualElementRenderer GetRenderer(this VisualElement self)
		{
			if (self == null)
				throw new ArgumentNullException(nameof(self));

			return Platform.GetRenderer(self);
		}
		internal static bool UseLegacyColorManagement<T>(this T element) where T : VisualElement, IElementConfiguration<T>
		{
			// Determine whether we're letting the VSM handle the colors or doing it the old way
			// or disabling the legacy color management and doing it the old-old (pre 2.0) way
			return !element.HasVisualStateGroups()
					&& element.OnThisPlatform().GetIsLegacyColorModeEnabled();
		}
	}
}