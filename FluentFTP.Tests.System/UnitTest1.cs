using System;
using Xunit;

namespace FluentFTP.Tests.System
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
			using (var client = new FtpClient("localhost"))
			{
				client.Connect();
				var list = client.GetListing();
				Assert.NotNull(list);
			}
        }
    }
}
