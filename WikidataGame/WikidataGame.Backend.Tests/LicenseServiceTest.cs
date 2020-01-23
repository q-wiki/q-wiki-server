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
            var result = await CommonsImageService.RetrieveImageInfoByUrlAsync("https://commons.wikimedia.org/wiki/Special:FilePath/Agujero_Negro_-_Sandra_Abigail_P%C3%A9rez_Gonz%C3%A1lez.jpg");
            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.DescriptionUrl));
            Assert.False(string.IsNullOrEmpty(result.ThumbUrl));
            Assert.False(string.IsNullOrEmpty(result.Url));
            Assert.False(string.IsNullOrEmpty(result.LicenseInfo.Artist.Value));
            Assert.False(string.IsNullOrEmpty(result.LicenseInfo.LicenseShortName.Value));
            Assert.False(string.IsNullOrEmpty(result.LicenseInfo.LicenseUrl.Value));
            Assert.False(string.IsNullOrEmpty(result.LicenseInfo.ObjectName.Value));
        }
    }
}
