using System.Collections;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.CustomAttributes;
using Microsoft.Maui.Dispatching;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Microsoft.Maui.Controls.Compatibility.Platform.Android.UnitTests
{
	[TestFixture]
	public class IsEnabledTests : PlatformTestFixture
	{
		static IEnumerable TestCases
		{
			get
			{
				// Generate IsEnabled = true cases
				foreach (var element in BasicElements)
				{
					element.IsEnabled = true;
					yield return CreateTestCase(element)
						.SetName($"{element.GetType().Name}_IsEnabled_{element.IsEnabled}");
				}

				// Generate IsEnabled = false cases
				foreach (var element in BasicElements)
				{
					element.IsEnabled = false;
					yield return CreateTestCase(element)
						.SetName($"{element.GetType().Name}_IsEnabled_{element.IsEnabled}");
				}
			}
		}

		[Test, Category("IsEnabled"), TestCaseSource(nameof(TestCases))]
		[Description("VisualElement enabled should match renderer enabled")]
		public async Task EnabledConsistent(VisualElement element)
		{
			await element.Dispatcher.DispatchAsync(() =>
			{
				using (var renderer = GetRenderer(element))
				{
					var expected = element.IsEnabled;
					var nativeView = renderer.View;

					ParentView(nativeView);

					// Check the container control
					Assert.That(renderer.View.Enabled, Is.EqualTo(expected));

					// Check the actual control
					var control = GetNativeControl(element);
					Assert.That(control.Enabled, Is.EqualTo(expected));

					UnparentView(nativeView);
				}
			});
		}
	}
}