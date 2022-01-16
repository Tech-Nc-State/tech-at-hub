using System;
using System.Linq;
using System.Text;
using FluentAssertions;
using Tech_HubAPI.Services;

using Xunit;
using Xunit.Abstractions;

namespace Tech_HubAPITest
{
    public class HashingServiceTest
    {
        private HashingService service;

        private readonly ITestOutputHelper output;

        public HashingServiceTest(ITestOutputHelper output)
        {
            this.output = output;
            this.service = new HashingService();
        }

        [Fact]
        public void GetSaltTest()
        {
            byte[] salt1 = service.GetSalt();
            byte[] salt2 = service.GetSalt();
            byte[] salt3 = service.GetSalt();

            salt1.Length.Should().Be(32);
            salt2.Length.Should().Be(32);
            salt3.Length.Should().Be(32);

            salt1.Should().NotEqual(salt2);
            salt2.Should().NotEqual(salt3);
            salt3.Should().NotEqual(salt1);
        }

        [Theory]
        [InlineData(new object[] { "password", "dd4ad1d3d6f001be8fc87b7d4789d69d58e7229b71c0aa8387dc78b24ad24b0c01ea44e84c15f92826989f9fd14c5032cc409c000b344e78dd1eca4b054a16af" })]
        [InlineData(new object[] { "Wolfpack123!", "5199f92bca07852ec548e211b0f3886b7f11d7aed3078ca3f9525a3b539ff014fa370a4ecd52e471bece65e6fa61734b02e818ebaf379788fa61f4577d0f5664" })]
        [InlineData(new object[] { "Very_Long*Password(&^%WITH+SPEciAL@!Characters", "f016990812e25660f1e867e0e7aabf93bf3795d56c49da8edf7d7762e8903bf6bef95e5a877b02db997a5915b00cc24eda4a44fe2b63bde9f8d4040165f5c0d3" })]
        public void HashPasswordTest(string plainText, string expectedHash)
        {
            // Used https://emn178.github.io/online-tools/sha512.html to get these SHA512 byte sequences

            byte[] salt = Enumerable.Repeat((byte)'a', 32).ToArray();

            byte[] hashedPassword = service.HashPassword(plainText, salt);
            string hashedPasswordText = BitConverter.ToString(hashedPassword);

            hashedPasswordText.Should().BeEquivalentTo(InsertDashes(expectedHash));
        }

        private string InsertDashes(string hex)
        {
            var newHex = new StringBuilder();
            for (int i = 0; i < hex.Length; i += 2)
            {
                newHex.Append(hex.Substring(i, 2));
                newHex.Append('-');
            }

            // remove the trailing dash
            newHex.Remove(newHex.Length - 1, 1);

            return newHex.ToString();
        }
    }
}
