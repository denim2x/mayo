// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.Maui.Controls.Core.UnitTests;
using NUnit.Framework;

namespace Microsoft.Maui.Controls.Xaml.UnitTests
{
	public partial class Gh7531 : ContentPage
	{
		public Gh7531() => InitializeComponent();
		public Gh7531(bool useCompiledXaml)
		{
			//this stub will be replaced at compile time
		}

		[TestFixture]
		class Tests
		{
			[Test]
			public void XamlOnlyResourceResolvesLocalAssembly([Values(false, true)] bool useCompiledXaml)
			{
				Gh7531 layout = null;
				Assert.DoesNotThrow(() => layout = new Gh7531(useCompiledXaml));
				var style = ((ResourceDictionary)layout.Resources["Colors"])["style"] as Style;
				Assert.That(style.TargetType, Is.EqualTo(typeof(Gh7531)));
			}
		}
	}
}