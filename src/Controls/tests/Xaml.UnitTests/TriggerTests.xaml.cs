using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Core.UnitTests;
using NUnit.Framework;

namespace Microsoft.Maui.Controls.Xaml.UnitTests
{
	public partial class TriggerTests : ContentPage
	{
		public TriggerTests()
		{
			InitializeComponent();
		}

		public TriggerTests(bool useCompiledXaml)
		{
			//this stub will be replaced at compile time
		}

		[TestFixture]
		public class Tests
		{
			[TestCase(false)]
			[TestCase(true)]
			public void ValueIsConverted(bool useCompiledXaml)
			{
				var layout = new TriggerTests(useCompiledXaml);
				Entry entry = layout.entry;
				Assert.NotNull(entry);

				var triggers = entry.Triggers;
				Assert.IsNotEmpty(triggers);
				var pwTrigger = triggers[0] as Trigger;
				Assert.AreEqual(Entry.IsPasswordProperty, pwTrigger.Property);
				Assert.AreEqual(true, pwTrigger.Value);
			}

			[TestCase(false)]
			[TestCase(true)]
			public void ValueIsConvertedWithPropertyCondition(bool useCompiledXaml)
			{
				var layout = new TriggerTests(useCompiledXaml);
				Entry entry = layout.entry1;
				Assert.NotNull(entry);

				var triggers = entry.Triggers;
				Assert.IsNotEmpty(triggers);
				var pwTrigger = triggers[0] as MultiTrigger;
				var pwCondition = pwTrigger.Conditions[0] as PropertyCondition;
				Assert.AreEqual(Entry.IsPasswordProperty, pwCondition.Property);
				Assert.AreEqual(true, pwCondition.Value);
			}
		}
	}
}