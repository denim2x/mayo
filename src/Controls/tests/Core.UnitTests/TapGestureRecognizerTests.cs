using System;
using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{

	public class TapGestureRecognizerTests : BaseTestFixture
	{
		[Fact]
		public void Constructor()
		{
			var tap = new TapGestureRecognizer();

			Assert.Null(tap.Command);
			Assert.Null(tap.CommandParameter);
			Assert.Equal(1, tap.NumberOfTapsRequired);
		}

		[Fact]
		public void CallbackPassesParameter()
		{
			var view = new View();
			var tap = new TapGestureRecognizer();
			tap.CommandParameter = "Hello";

			object result = null;
			tap.Command = new Command(o => result = o);

			tap.SendTapped(view);
			Assert.Equal(result, tap.CommandParameter);
		}
	}
}
