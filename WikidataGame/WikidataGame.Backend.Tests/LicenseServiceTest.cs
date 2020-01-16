using System;
using System.Collections.Generic;
using System.Text;
using WikidataGame.Backend.Services;
using Xunit;

namespace WikidataGame.Backend.Tests
{
    public class LicenseServiceTest
    {
        [Fact]
        public async void RetrieveLicenseInfoByUrlAsync_TestFile_Succeeds()
        {
            var result = await CommonsLicenseService.RetrieveLicenseInfoByUrlAsync("https://commons.wikimedia.org/wiki/File:Sa-warthog.jpg");
            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.Artist.Value));

        }
    }
}
