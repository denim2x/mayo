using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls.Core.UnitTests;
using NUnit.Framework;
namespace Microsoft.Maui.Controls.Xaml.UnitTests
{
	public class Bz60575Helpers
	{
		public static readonly BindableProperty CollectionProperty =
			BindableProperty.Create("Collection", typeof(IList<string>), typeof(Bz60575Helpers), default(IList<string>), defaultValueCreator: (b) => new List<string>());

		public static IList<string> GetCollection(BindableObject bindable)
		{
			return (IList<string>)bindable.GetValue(CollectionProperty);
		}

		public static void SetCollection(BindableObject bindable, IList<string> value)
		{
			bindable.SetValue(CollectionProperty, value);
		}

		public static readonly BindableProperty Collection2Property =
			BindableProperty.Create("Collection2", typeof(IList<string>), typeof(Bz60575Helpers), default(IList<string>), defaultValueCreator: (b) => new List<string>());

		public static IList<string> GetCollection2(BindableObject bindable)
		{
			return (IList<string>)bindable.GetValue(Collection2Property);
		}

		public static void SetCollection2(BindableObject bindable, IList<string> value)
		{
			bindable.SetValue(Collection2Property, value);
		}

	}

	public partial class Bz60575 : ContentPage
	{
		public Bz60575()
		{
			InitializeComponent();
		}

		public Bz60575(bool useCompiledXaml)
		{
			//this stub will be replaced at compile time
		}

		public static readonly BindableProperty CollectionProperty =
			BindableProperty.Create("Collection", typeof(IList<string>), typeof(Bz60575), default(IList<string>), defaultValueCreator: (b) => new List<string>());

		public IList<string> Collection
		{
			get { return (IList<string>)GetValue(CollectionProperty); }
			set { SetValue(CollectionProperty, value); }
		}

		public static readonly BindableProperty Collection2Property =
			BindableProperty.Create("Collection2", typeof(IList<string>), typeof(Bz60575), default(IList<string>), defaultValueCreator: (b) => new List<string>());

		public IList<string> Collection2
		{
			get { return (IList<string>)GetValue(Collection2Property); }
			set { SetValue(Collection2Property, value); }
		}

		[TestFixture]
		class Tests
		{
			[TestCase(true), TestCase(false)]
			public void CollectionProperties(bool useCompiledXaml)
			{
				var layout = new Bz60575(useCompiledXaml);

				//attached BP
				var col = layout.GetValue(Bz60575Helpers.CollectionProperty) as IList<string>;
				Assert.That(col.Count, Is.EqualTo(2));

				//attached BP with a single element
				col = layout.GetValue(Bz60575Helpers.Collection2Property) as IList<string>;
				Assert.That(col.Count, Is.EqualTo(1));

				//normal BP
				Assert.That(layout.Collection.Count, Is.EqualTo(3));

				//normal BP with a single element
				Assert.That(layout.Collection2.Count, Is.EqualTo(1));
			}
		}
	}
}