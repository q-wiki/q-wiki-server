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
            var result = await CommonsLicenseService.RetrieveLicenseInfoByUrlAsync("https://commons.wikimedia.org/wiki/Special:FilePath/Agujero_Negro_-_Sandra_Abigail_P%C3%A9rez_Gonz%C3%A1lez.jpg");
            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.Artist.Value));
            Assert.False(string.IsNullOrEmpty(result.LicenseShortName.Value));
            Assert.False(string.IsNullOrEmpty(result.LicenseUrl.Value));
            Assert.False(string.IsNullOrEmpty(result.ObjectName.Value));
        }
    }
}
