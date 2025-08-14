//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Caching.Memory;
//using Microsoft.Extensions.Logging;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TaskService.API.Controllers;
//using TaskService.API.Models.Currency;

//namespace TaskService.Tests
//{
//    public class CurrencyControllerTests
//    {
//        [Fact]
//        public async Task GetCurrencyRates_ReturnsCachedData()
//        {
//            // Arrange
//            var mockHttpFactory = new Mock<IHttpClientFactory>();
//            var mockCache = new Mock<IMemoryCache>();
//            var mockLogger = new Mock<ILogger<CurrencyController>>();

//            var expectedRates = new CurrencyRates { /* тестовые данные */ };

//            object cachedValue = expectedRates;
//            mockCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedValue))
//                     .Returns(true);

//            var controller = new CurrencyController(mockHttpFactory.Object, mockCache.Object, mockLogger.Object);

//            // Act
//            var result = await controller.GetCurrencyRates();

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result.Result);
//            var returnValue = Assert.IsType<CurrencyRates>(okResult.Value);
//            Assert.Equal(expectedRates, returnValue);

//            mockLogger.Verify(
//                x => x.Log(
//                    LogLevel.Information,
//                    It.IsAny<EventId>(),
//                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("cached")),
//                    null,
//                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
//                Times.Once);
//        }
//    }
//}
