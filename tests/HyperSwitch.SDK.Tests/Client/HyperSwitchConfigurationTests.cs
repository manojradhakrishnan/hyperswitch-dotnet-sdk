using System;
using HyperSwitch.SDK.Client;
using Xunit;

namespace HyperSwitch.SDK.Tests.Client
{
    public class HyperSwitchConfigurationTests
    {
        [Fact]
        public void DefaultConstructor_SetsDefaultValues()
        {
            // Act
            var config = new HyperSwitchConfiguration();

            // Assert
            Assert.Equal("https://sandbox.hyperswitch.io", config.BaseUrl);
            Assert.Null(config.ApiKey);
            Assert.Null(config.HttpClient);
            Assert.Equal(TimeSpan.FromSeconds(100), config.Timeout);
        }

        [Fact]
        public void Constructor_WithApiKey_SetsApiKey()
        {
            // Arrange
            var apiKey = "test_api_key";

            // Act
            var config = new HyperSwitchConfiguration(apiKey);

            // Assert
            Assert.Equal(apiKey, config.ApiKey);
            Assert.Equal("https://sandbox.hyperswitch.io", config.BaseUrl);
        }

        [Fact]
        public void Constructor_WithApiKeyAndBaseUrl_SetsApiKeyAndBaseUrl()
        {
            // Arrange
            var apiKey = "test_api_key";
            var baseUrl = "https://api.example.com";

            // Act
            var config = new HyperSwitchConfiguration(apiKey, baseUrl);

            // Assert
            Assert.Equal(apiKey, config.ApiKey);
            Assert.Equal(baseUrl, config.BaseUrl);
        }

        [Fact]
        public void Constructor_WithNullApiKey_ThrowsArgumentNullException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new HyperSwitchConfiguration(null));
            Assert.Equal("apiKey", exception.ParamName);
        }

        [Fact]
        public void Constructor_WithNullBaseUrl_ThrowsArgumentNullException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new HyperSwitchConfiguration("api_key", null));
            Assert.Equal("baseUrl", exception.ParamName);
        }
    }
} 