using Android.Content;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Controls.Platform;

namespace Microsoft.Maui.Controls.Compatibility.Platform.Android
{
	// We don't currently have a replacement for this
#pragma warning disable CS0618 // Type or member is obsolete
	public class NativeViewWrapperRenderer : ViewRenderer<NativeViewWrapper, global::Android.Views.View>
#pragma warning restore CS0618 // Type or member is obsolete
	{
		public NativeViewWrapperRenderer(Context context) : base(context)
		{
		}

		public override SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
		{
			if (Element?.GetDesiredSizeDelegate == null)
				return base.GetDesiredSize(widthConstraint, heightConstraint);

			// The user has specified a different implementation of GetDesiredSizeDelegate
			SizeRequest? result = Element.GetDesiredSizeDelegate(this, widthConstraint, heightConstraint);

			// If the delegate returns a SizeRequest, we use it; if it returns null,
			// fall back to the default implementation
			return result ?? base.GetDesiredSize(widthConstraint, heightConstraint);
		}

		// not called by the view wrapper renderer
		protected override global::Android.Views.View CreateNativeControl()
		{
			return new global::Android.Views.View(Context);
		}

		protected override void OnElementChanged(ElementChangedEventArgs<NativeViewWrapper> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement == null)
			{
				SetNativeControl(Element.NativeView);
				Control.LayoutChange += (sender, args) => ((IVisualElementController)Element)?.InvalidateMeasure(InvalidationTrigger.MeasureChanged);
			}
		}

		protected override void OnLayout(bool changed, int l, int t, int r, int b)
		{
			if (Element?.OnLayoutDelegate == null)
			{
				base.OnLayout(changed, l, t, r, b);
				return;
			}

			// The user has specified a different implementation of OnLayout
			bool handled = Element.OnLayoutDelegate(this, changed, l, t, r, b);

			// If the delegate wasn't able to handle the request, fall back to the default implementation
			if (!handled)
				base.OnLayout(changed, l, t, r, b);
		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			if (Element?.OnMeasureDelegate == null)
			{
				base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
				return;
			}

			// The user has specified a different implementation of OnMeasure
			bool handled = Element.OnMeasureDelegate(this, widthMeasureSpec, heightMeasureSpec);

			// If the delegate wasn't able to handle the request, fall back to the default implementation
			if (!handled)
				base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
		}

		protected override bool ManageNativeControlLifetime => false;
	}
}