using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Services;
using Xunit;

namespace WikidataGame.Backend.Tests
{
    public class LicenseServiceTest
    {
        [Fact]
        public async void RetrieveLicenseInfoByUrlAsync_TestFile_Succeeds()
        {
            var mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new AutomapperProfile(null))
            ).CreateMapper();
            var result = await CommonsImageService.RetrieveImageInfoByUrlAsync("https://commons.wikimedia.org/wiki/Special:FilePath/Agujero_Negro_-_Sandra_Abigail_P%C3%A9rez_Gonz%C3%A1lez.jpg", mapper);
            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.DescriptionUrl));
            Assert.False(string.IsNullOrEmpty(result.ThumbUrl));
            Assert.False(string.IsNullOrEmpty(result.Url));
            Assert.False(string.IsNullOrEmpty(result.Artist));
            Assert.False(string.IsNullOrEmpty(result.LicenseName));
            Assert.False(string.IsNullOrEmpty(result.LicenseUrl));
            Assert.False(string.IsNullOrEmpty(result.Name));
        }
    }
}
