using System;
using Xunit;
using System.Text;

namespace BikeChain.Tests
{
    public class BlockTests
    {
        [Fact]
        public void BlockToString()
        {
            const string PREV_HASH = "previoushash";
            const string THIS_HASH = "thishash";
            const string FAKE_DATA = "fakedata";

            var timestamp = DateTime.UtcNow;
            Block block = new Block(timestamp, Encoding.UTF8.GetBytes(PREV_HASH), Encoding.UTF8.GetBytes(THIS_HASH), Encoding.UTF8.GetBytes(FAKE_DATA));

            string representation = block.ToString();

            Assert.Contains(timestamp.Ticks.ToString(), representation);
            Assert.Contains(Convert.ToBase64String(Encoding.UTF8.GetBytes(PREV_HASH)), representation);
            Assert.Contains(Convert.ToBase64String(Encoding.UTF8.GetBytes(THIS_HASH)), representation);
            Assert.Contains(Convert.ToBase64String(Encoding.UTF8.GetBytes(FAKE_DATA)), representation);
        }
    }
}
