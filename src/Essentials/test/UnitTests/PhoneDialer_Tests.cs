using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.ApplicationModel.Communication;
using Xunit;

namespace Tests
{
	public class PhoneDialer_Tests
	{
		[Fact]
		public void Dialer_Open_Fail_On_NetStandard() =>
			Assert.Throws<NotImplementedInReferenceAssemblyException>(() => PhoneDialer.Open("1234567890"));
	}
}
