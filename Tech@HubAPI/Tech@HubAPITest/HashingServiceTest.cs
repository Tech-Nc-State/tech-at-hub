using System;
using System.Linq;
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

        [Fact]
        public void HashPasswordTest()
        {
            byte[] salt = Enumerable.Repeat<byte>(0xFF, 32).ToArray();


            String password1 = "password";
            String password2 = "Wolfpack123!";
            String password3 = "Very_Long*Password(&^%WITH+SPEciAL@!Characters";

            // Used https://emn178.github.io/online-tools/sha512.html to get these SHA512 byte sequences
            String actualHashedPassword1 = "b6-41-61-ad-24-d1-48-8d-02-82-82-76-04-a2-b4-68-c8-42-e6-cd-2b-21-9f-12-0a-c1-e3-4d-eb-cf-ff-20-66-b0-5f-8c-7a-1a-71-e5-ce-88-46-3c-36-ba-b0-1a-a8-81-4e-1b-88-d5-b4-aa-ad-bd-c3-62-3e-97-e9-1c";
            String actualHashedPassword2 = "c8-f3-19-af-e8-23-10-2e-80-29-36-2c-71-c0-13-73-d7-97-eb-39-a5-59-dd-68-eb-24-d8-c0-83-ee-1f-f7-8e-a3-7b-64-b4-57-95-25-33-c2-64-01-ac-5b-b3-5f-64-dd-bd-6e-bb-87-cf-a2-57-6f-5e-ef-1a-98-5b-5c";
            String actualHashedPassword3 = "68-74-de-b2-1e-c6-cb-d6-b0-3d-b3-a0-db-9b-7a-41-64-f9-5f-49-7a-22-85-d7-b7-3f-6c-d2-f6-55-68-bc-27-ad-7f-d7-01-3f-1a-9e-05-59-39-d2-4b-70-97-1f-dd-97-bb-c1-c7-4e-2d-3e-74-9c-58-df-27-eb-88-15";

            byte[] hashedPassword1 = service.HashPassword(password1, salt);
            byte[] hashedPassword2 = service.HashPassword(password2, salt);
            byte[] hashedPassword3 = service.HashPassword(password3, salt);

            actualHashedPassword1.Should().BeEquivalentTo(BitConverter.ToString(hashedPassword1));
            actualHashedPassword2.Should().BeEquivalentTo(BitConverter.ToString(hashedPassword2));
            actualHashedPassword3.Should().BeEquivalentTo(BitConverter.ToString(hashedPassword3));
        }
    }
}
