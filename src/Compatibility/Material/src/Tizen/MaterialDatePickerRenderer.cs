using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility.Material.Tizen;
using Microsoft.Maui.Controls.Compatibility.Material.Tizen.Native;
using Microsoft.Maui.Controls.Platform.Tizen;

[assembly: ExportRenderer(typeof(DatePicker), typeof(MaterialDatePickerRenderer), new[] { typeof(VisualMarker.MaterialVisual) }, Priority = short.MinValue)]
namespace Microsoft.Maui.Controls.Compatibility.Material.Tizen
{
	public class MaterialDatePickerRenderer : DatePickerRenderer
	{
		Color _defaultTitleColor = Color.Black;

		protected override ElmSharp.Entry CreateNativeControl()
		{
			return new MPicker(Forms.NativeParent);
		}

		protected override void OnDateTimeChanged(object sender, Platform.Tizen.Native.DateChangedEventArgs dcea)
		{
			Element.Date = dcea.NewDate;
			if (Control is MPicker mp)
			{
				mp.Placeholder = dcea.NewDate.ToString(Element.Format);
			}
		}

		protected override void UpdateDate()
		{
			if (Control is MPicker mp)
			{
				mp.Placeholder = Element.Date.ToString(Element.Format);
			}
		}

		protected override void UpdateTextColor()
		{
			if (Control is MPicker mp)
			{
				if (Element.TextColor.IsDefault)
				{
					mp.PlaceholderColor = _defaultTitleColor.ToPlatform();
				}
				else
				{
					mp.PlaceholderColor = Element.TextColor.ToPlatform();
				}
			}
		}
	}
}