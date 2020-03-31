using System;
using System.Threading.Tasks;
using Recruitment.Core;
using Recruitment.Services;
using Xunit;

namespace Recruitment.Tests
{
    public class ServicesUnitTests
    {
        [Fact]
        public async Task BlobStorageServiceThrowsExceptionWhenFileNameIsNull()
        {
            await Assert.ThrowsAsync<Exception>(() => BlobStorageService.UploadToBlob(null, null));
        }

        [Fact]
        public async Task SendGridServiceThrowsExceptionWhenWrongApiKeyIsUsed()
        {
            await Assert.ThrowsAsync<Exception>(() =>
                SendGridService.SendEmail("receiver@email.pl", "testoffer", "badkey"));
        }

        [Fact]
        public void HrMemberConstructorWorks()
        {
            string email = "sample@google.com";
            string name = "Andrew";
            string surname = "Golota";
            var hrMember = new HrMember(name, surname, email);
            Assert.Equal(hrMember.Email, email);
            Assert.Equal(hrMember.Name,name);
            Assert.Equal(hrMember.Surname,surname);
        }
    }
}