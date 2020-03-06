using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RemitanoDevTask.Controllers;
using RemitanoDevTask.Models;

namespace RemitanoDevTask.UnitTest.Controllers
{
    [TestClass()]
    public class NumberValidationTests
    {
        //private readonly Mock<ICategoryRepository> _mockRepo;
        //private readonly HomeController _controller;

        public NumberValidationTests()
        {
            //_mockRepo = new Mock<ICategoryRepository>();
            //_controller = homeController;
        }
        
        [TestMethod]
        public void Index_ActionExecutes_ReturnsViewForIndex()
        {
            var result = new ContactController().Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}