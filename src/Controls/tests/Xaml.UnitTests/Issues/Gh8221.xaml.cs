// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Core.UnitTests;
using NUnit.Framework;

namespace Microsoft.Maui.Controls.Xaml.UnitTests
{
	public class Gh8221VM
	{
		public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string> { { "EntryOne", "One" }, { "EntryTwo", "Two" } };
	}

	public partial class Gh8221 : ContentPage
	{
		public Gh8221() => InitializeComponent();
		public Gh8221(bool useCompiledXaml)
		{
			//this stub will be replaced at compile time
		}

		[TestFixture]
		class Tests
		{
			[Test]
			public void BindingWithMultipleIndexers([Values(false, true)] bool useCompiledXaml)
			{
				if (useCompiledXaml)
					MockCompiler.Compile(typeof(Gh8221));
				var layout = new Gh8221(useCompiledXaml) { BindingContext = new Gh8221VM() };
				Assert.That(layout.entryone.Text, Is.EqualTo("One"));
				Assert.That(layout.entrytwo.Text, Is.EqualTo("Two"));
			}
		}
	}
}
