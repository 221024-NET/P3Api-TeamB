using System.Threading.Tasks;
using Castle.Core.Logging;
using ECommerce.API.Controllers;
using ECommerce.Data;
using ECommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Moq;
using Xunit;

namespace ECommerce.Tests
{
    public class ProductControllerTests
    {

        [Fact]
        public void GetOne_SendsDesiredProductInResponseIfProvidedIdIsInDb()
        {
            var cMock = new Mock<IContext>();
            cMock.Setup(m => m.GetProductById(1)).Returns(Task.FromResult(new Product { ProductId = 1, ProductName = "Banana", ProductDescription = "Potassium", ProductImage = "", ProductQuantity = 50, ProductPrice = 2}));
            ProductController controller = new ProductController(cMock.Object);
            int desiredProductId = 1;

            var result = controller.GetOne(desiredProductId);

            Assert.IsType<Product>(result.Result.Value);
            Assert.Equal(desiredProductId, result.Result.Value.ProductId);
            Assert.Equal("Banana", result.Result.Value.ProductName);
        }

        [Fact]
        public void GetOne_SendsANullResponseOnAnIdNotInTheDb()
        {
            var cMock = new Mock<IContext>();
            cMock.Setup(m => m.GetProductById(2)).Returns(Task.FromResult<Product>(null));
            ProductController controller = new ProductController(cMock.Object);
            int desiredProductId = 2;

            var result = controller.GetOne(desiredProductId);

            Assert.Null(result.Result.Value);
        }

        [Fact]
        public void GetOne_SendsANullResponseWhenDbIsEmpty()
        {
            var cMock = new Mock<IContext>();
            cMock.Setup(m => m.GetProductById(1)).Returns(Task.FromResult<Product>(null));
            ProductController controller = new ProductController(cMock.Object);
            int desiredProductId = 1;

            var result = controller.GetOne(desiredProductId);

            Assert.Null(result.Result.Value);
        }

        [Fact]
        public void GetAll_SendsListOfProductsWhenRequested()
        {
            var cMock = new Mock<IContext>();
            var mList = new List<Product>()
            {
                new Product { ProductId = 1, ProductName = "Banana", ProductQuantity = 30, ProductPrice = 3, ProductDescription = "Oooh banana!", ProductImage = "" },
                new Product { ProductId = 2, ProductName = "Honeydew", ProductQuantity = 50, ProductPrice = 5, ProductDescription = "The best fruit", ProductImage = "" }
            };
            cMock.Setup<Task<IEnumerable<Product>>>(m => m.GetAllProducts()).Returns(Task.FromResult<IEnumerable<Product>>(mList));
            ProductController controller = new ProductController(cMock.Object);

            var result = controller.GetAll();

            Assert.IsType<List<Product>>(result.Result.Value);
        }

        [Fact]
        public void GetAll_SendsEmptyListIfRequestedWhileDbIsEmpty()
        {
            var cMock = new Mock<IContext>();
            cMock.Setup(m => m.GetAllProducts()).Returns(Task.FromResult<IEnumerable<Product>>(new List<Product>()));
            ProductController controller = new ProductController(cMock.Object);

            var result = controller.GetAll();

            Assert.Empty(result.Result.Value);
        }

        [Fact]
        public void Purchase_UpdatesProductsProvidedSoLongAsThereIsSufficientQuantity()
        {
            var cMock = new Mock<IContext>();
            cMock.Setup(m => m.GetProductById(1)).Returns(Task.FromResult(new Product { ProductId = 1, ProductName = "Banana", ProductQuantity = 10, ProductPrice = 3, ProductDescription = "Oooh banana!", ProductImage = "" }));
            cMock.Setup(m => m.GetProductById(2)).Returns(Task.FromResult(new Product { ProductId = 2, ProductName = "Honeydew", ProductQuantity = 5, ProductPrice = 5, ProductDescription = "The best fruit", ProductImage = "" }));
            cMock.Setup(m => m.UpdateProduct(It.IsAny<Product>()))
                .Callback((Product p) => {
                    p.ProductQuantity = 1;
                }).Verifiable();
            ProductController controller = new ProductController(cMock.Object);
            ProductDTO newProd = new ProductDTO { id = 1, quantity = 10 };
            ProductDTO newProd2 = new ProductDTO { id = 2, quantity = 5 };
            ProductDTO[] purchasedProducts = new ProductDTO[2];
            purchasedProducts[0] = newProd;
            purchasedProducts[1] = newProd2;

            var result = controller.Purchase(purchasedProducts);

            cMock.Verify(m => m.UpdateProduct(It.IsAny<Product>()), Times.Exactly(2));
        }

        [Fact]
        public void Purchase_SituationallyUpdatesProductsIfQuantityIsInsufficient()
        {
            var cMock = new Mock<IContext>();
            cMock.Setup(m => m.GetProductById(1)).Returns(Task.FromResult(new Product { ProductId = 1, ProductName = "Banana", ProductQuantity = 2, ProductPrice = 3, ProductDescription = "Oooh banana!", ProductImage = "" }));
            cMock.Setup(m => m.GetProductById(2)).Returns(Task.FromResult(new Product { ProductId = 2, ProductName = "Honeydew", ProductQuantity = 1, ProductPrice = 5, ProductDescription = "The best fruit", ProductImage = "" }));
            cMock.Setup(m => m.UpdateProduct(It.IsAny<Product>()))
                .Callback((Product p) => {
                    p.ProductQuantity = p.ProductQuantity - 1;
                }).Verifiable();
            ProductController controller = new ProductController(cMock.Object);
            ProductDTO newProd = new ProductDTO(1, 1);
            ProductDTO newProd2 = new ProductDTO(2, 5);
            ProductDTO[] purchasedProducts = new ProductDTO[2];
            purchasedProducts[0] = newProd;
            purchasedProducts[1] = newProd2;

            var result = controller.Purchase(purchasedProducts);

            cMock.Verify(m => m.UpdateProduct(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public void Purchase_NeverUpdatesProductsWhenProductsProvidedAreNotInDb()
        {
            var cMock = new Mock<IContext>();
            cMock.Setup(m => m.GetProductById(5)).Returns(Task.FromResult<Product>(null));
            cMock.Setup(m => m.GetProductById(6)).Returns(Task.FromResult<Product>(null));
            cMock.Setup(m => m.UpdateProduct(It.IsAny<Product>()))
                .Callback((Product p) => {
                    p.ProductQuantity = p.ProductQuantity - 1;
                }).Verifiable();
            ProductController controller = new ProductController(cMock.Object);
            ProductDTO newProd = new ProductDTO { id = 5, quantity = 10 };
            ProductDTO newProd2 = new ProductDTO { id = 6, quantity = 5 };
            ProductDTO[] purchasedProducts = new ProductDTO[2];
            purchasedProducts[0] = newProd;
            purchasedProducts[1] = newProd2;

            var result = controller.Purchase(purchasedProducts);

            cMock.Verify(m => m.UpdateProduct(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public void Purchase_SendsNullResponseWhenProductsProvidedAreNotInDb()
        {
            var cMock = new Mock<IContext>();
            cMock.Setup(m => m.GetProductById(5)).Returns(Task.FromResult<Product>(null));
            cMock.Setup(m => m.GetProductById(6)).Returns(Task.FromResult<Product>(null));
            cMock.Setup(m => m.UpdateProduct(It.IsAny<Product>()))
                .Callback((Product p) => {
                    p.ProductQuantity = p.ProductQuantity - 1;
                }).Verifiable();
            ProductController controller = new ProductController(cMock.Object);
            ProductDTO newProd = new ProductDTO { id = 5, quantity = 10 };
            ProductDTO newProd2 = new ProductDTO { id = 6, quantity = 5 };
            ProductDTO[] purchasedProducts = new ProductDTO[2];
            purchasedProducts[0] = newProd;
            purchasedProducts[1] = newProd2;

            var result = controller.Purchase(purchasedProducts);

            Assert.Null(result.Result.Value);
        }
    }
}
