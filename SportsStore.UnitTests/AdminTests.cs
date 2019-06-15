using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class AdminTests
    {

        [TestMethod]
        public void Index_Contains_All_Products()
        {
            // Arrange - create the mock repository
            var mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
            });

            // Arrange - create a controller
            var target = new AdminController(mock.Object);

            // Action
            var result = ((IEnumerable<Product>)target.Index().ViewData.Model).ToArray();

            // Assert
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual("P1", result[0].Name);
            Assert.AreEqual("P2", result[1].Name);
            Assert.AreEqual("P3", result[2].Name);
        }

        [TestMethod]
        public void Can_Edit_Product()
        {

            // Arrange - create the mock repository
            var mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
            });

            // Arrange - create the controller
            var target = new AdminController(mock.Object);

            // Act
            var p1 = target.Edit(1).ViewData.Model as Product;
            var p2 = target.Edit(2).ViewData.Model as Product;
            var p3 = target.Edit(3).ViewData.Model as Product;

            // Assert
            Assert.AreEqual(1, p1.ProductID);
            Assert.AreEqual(2, p2.ProductID);
            Assert.AreEqual(3, p3.ProductID);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Product()
        {

            // Arrange - create the mock repository
            var mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
            });

            // Arrange - create the controller
            var target = new AdminController(mock.Object);

            // Act
            var result = (Product)target.Edit(4).ViewData.Model;

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            // Arrange - create mock repository
            var mock = new Mock<IProductsRepository>();
            // Arrange - create the controller
            var target = new AdminController(mock.Object);
            // Arrange - create a product
            var product = new Product { Name = "Test" };

            // Act - try to save the product
            var result = target.Edit(product);

            // Assert - check that the repository was called
            mock.Verify(m => m.SaveProduct(product));

            // Assert - check the method result type
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {

            // Arrange - create mock repository
            var mock = new Mock<IProductsRepository>();
            // Arrange - create the controller
            var target = new AdminController(mock.Object);
            // Arrange - create a product
            var product = new Product { Name = "Test" };
            // Arrange - add an error to the model state
            target.ModelState.AddModelError("error", "error");

            // Act - try to save the product
            var result = target.Edit(product);

            // Assert - check that the repository was not called
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());
            // Assert - check the method result type
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}