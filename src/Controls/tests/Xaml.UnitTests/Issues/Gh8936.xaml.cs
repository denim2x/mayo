// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Core.UnitTests;
using NUnit.Framework;

namespace Microsoft.Maui.Controls.Xaml.UnitTests
{
	public class Dict : Dictionary<string, string> { }
	public class Gh8936VM
	{
		public Dict Data { get; set; } = new Dict { { "Key", "Value" } };
	}

	public partial class Gh8936 : ContentPage
	{
		public Gh8936() => InitializeComponent();
		public Gh8936(bool useCompiledXaml)
		{
			//this stub will be replaced at compile time
		}

		[TestFixture]
		class Tests
		{
			[Test]
			public void IndexerBindingOnSubclasses([Values(false, true)] bool useCompiledXaml)
			{
				var layout = new Gh8936(useCompiledXaml) { BindingContext = new Gh8936VM() };
				Assert.That(layout.entry0.Text, Is.EqualTo("Value"));
				layout.entry0.Text = "Bar";
				Assert.That(layout.entry0.Text, Is.EqualTo("Bar"));
				Assert.That(((Gh8936VM)layout.BindingContext).Data["Key"], Is.EqualTo("Bar"));
			}
		}
	}
}
