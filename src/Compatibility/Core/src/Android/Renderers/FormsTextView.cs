using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using AndroidX.AppCompat.Widget;

namespace Microsoft.Maui.Controls.Compatibility.Platform.Android
{
	public class FormsTextView : AppCompatTextView
	{
		public FormsTextView(Context context) : base(context)
		{
		}

		[Obsolete]
		public FormsTextView(Context context, IAttributeSet attrs) : base(context, attrs)
		{
		}

		[Obsolete]
		public FormsTextView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
		{
		}

		[Obsolete]
		protected FormsTextView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

		[Obsolete]
		public void SkipNextInvalidate()
		{
		}
	}
}