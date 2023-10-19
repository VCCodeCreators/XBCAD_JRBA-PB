using JRBAWebApplication2.Controllers;
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
            ActionResult result = controller.Estimations();//model,form

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
            ActionResult result = controller.Estimations();//mode;, form

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
	        // Arrange: Set up any necessary dependencies, data, or conditions for the test
	        HomeController controller = new HomeController();

	        // Act: Simulate an interaction between components
	        ViewResult result = controller.Estimations() as ViewResult;

	        // Assert: Verify the result or behavior
	        Assert.IsNotNull(result);
	        // Add more assertions as needed to check specific behavior
        }
        //------------------------------------------------------------------------------------------------------------------\\
        [TestMethod]
        [TestCategory("Regression")]
        public void MyRegressionTest()
        {
            // This test method seems incomplete, and the code is commented out.
            
        }
        //------------------------------------------------------------------------------------------------------------------\\
        [TestMethod]
        [TestCategory("Functional")]
        public void MyFunctionalTest()
        {
            // This is where you would write functional test code.
            
        }
        //------------------------------------------------------------------------------------------------------------------\\
        class CalculationModels
        {
            // If CalculationModels is a class used in your code, you can define it here.
        }
    }
}
