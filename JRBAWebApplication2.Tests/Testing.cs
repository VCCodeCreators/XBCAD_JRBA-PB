using JRBAWebApplication2.Controllers;
using JRBAWebApplication2.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Mvc;

namespace JRBAWebApplication2.Tests
{
    [TestClass]
    public class Testing
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void EstimationsUnitTest()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ActionResult result = controller.Estimations() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
        //------------------------------------------------------------------------------------------------------------------\\
        [TestMethod]
        [TestCategory("Unit")]
        public void EstimationsPostTest_ValidData()
        {
            // Arrange
            HomeController controller = new HomeController();
            CalculationModels model = new CalculationModels();
            FormCollection form = new FormCollection
            {
                { "BasinSelection", "Komati" },
                { "CropSelection", "Sugar Cane" },
                { "cropSize", "100" }
            };

            // Act
            ActionResult result = controller.Estimations(model,form);//model,form

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.AreEqual("R1200000 p/m", viewResult.ViewBag.EstimatedAmount);
        }
        //------------------------------------------------------------------------------------------------------------------\\
        [TestMethod]
        [TestCategory("Unit")]
        public void EstimationsPostTest_InvalidData()
        {
            // Arrange
            HomeController controller = new HomeController();
            CalculationModels model = new CalculationModels();
            FormCollection form = new FormCollection
            {
                { "BasinSelection", "InvalidBasin" },
                { "CropSelection", "InvalidCrop" },
                { "cropSize", "invalid" }
            };

            // Act
            ActionResult result = controller.Estimations(model,form);//mode;, form

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.AreEqual(" Error: Invalid Input", viewResult.ViewBag.EstimatedAmount);
        }
        //------------------------------------------------------------------------------------------------------------------\\
        [TestMethod]
        [TestCategory("Integration")]
        public void MyIntegrationTest()
        {
	        // Arrange
	        HomeController controller = new HomeController();

	        // Act
	        ViewResult result = controller.Estimations() as ViewResult;

	        // Assert
	        Assert.IsNotNull(result);
        }
        //------------------------------------------------------------------------------------------------------------------\\
        [TestMethod]
        [TestCategory("Regression")]
        public void MyRegressionTest()
        {
            
        }
        //------------------------------------------------------------------------------------------------------------------\\
        [TestMethod]
        [TestCategory("Functional")]
        public void MyFunctionalTest()
        {
            
            
        }
        //------------------------------------------------------------------------------------------------------------------\\
      
    }
}
//----------------------------------------...ooo000 END OF FILE 000ooo...----------------------------------------//