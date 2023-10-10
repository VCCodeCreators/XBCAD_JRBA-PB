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
			ViewResult result = controller.Estimations() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}
		//-------------------------------------------------------------------------------------------------------------------\\

		[TestMethod]
		[TestCategory("Integration")]
		public void MyIntegrationTest()
		{
			// Integration test code here
			// Arrange
			HomeController controller = new HomeController();

			// Act: Simulate an interaction between components
			ViewResult result = controller.Estimations() as ViewResult;

			// Assert: Verify the result or behavior
			Assert.IsNotNull(result);
		}
		//-------------------------------------------------------------------------------------------------------------------\\

		[TestMethod]
		[TestCategory("Regression")]
		public void MyRegressionTest()
		{
			// Arrange: Prepare data and an existing function
			int a = 5;
			int b = 7;
			HomeController controller = new HomeController();

			// Act: Call the existing function
			//int result = controller.Estimations.Add(a, b);

			// Assert: Verify the result
		//	Assert.AreEqual(12, result);
			// Regression test code here
		}
		//-------------------------------------------------------------------------------------------------------------------\\

		[TestMethod]
		[TestCategory("Functional")]
		public void MyFunctionalTest()
		{
			// Functional test code here
		}
		//-------------------------------------------------------------------------------------------------------------------\\

	}
}
//-----------------------------------------------------------End of File--------------------------------------------------------------\\

