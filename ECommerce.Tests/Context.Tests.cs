using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq.EntityFrameworkCore;
using Moq;
using ECommerce.Data;
using ECommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Tests
{
    public class ContextTests : IClassFixture<ContextFixture>
    {
        private readonly ContextFixture _fixture;

        public ContextTests(ContextFixture fixture)
        {
            this._fixture = fixture;
        }

        [Fact]
        public void GetProductById_RetrievesGivenProduct()
        {
            var result = _fixture.context.GetProductById(1);

            Assert.IsType<Product>(result.Result);
            Assert.Equal(1, result.Result.ProductId);
        }

        [Fact]
        public void GetProductById_ReturnsNullWhenGivenProductNotInDb()
        {
            var result = _fixture.context.GetProductById(4);

            Assert.Null(result.Result);
        }

        [Fact]
        public void GetProductById_ReturnsNullOnEmptyDb()
        {
            _fixture.EmptyProductTable();

            var result = _fixture.context.GetProductById(1);

            Assert.Null(result.Result);
        }

        [Fact]
        public void GetAllProducts_RetrievesAllProducts()
        {
            var result = _fixture.context.GetAllProducts();

            Assert.IsType<List<Product>>(result.Result);
        }

        [Fact]
        public void GetAllProducts_ReturnsEmptyListWhenDbIsEmpty()
        {
            _fixture.EmptyProductTable();

            var result = _fixture.context.GetAllProducts();

            Assert.IsType<List<Product>>(result.Result);
            Assert.Equal(0, result.Result.Count());
        }

        [Fact]
        public void UpdateProduct_ProperlyUpdatesGivenProductWithNewProduct()
        {
            Product newProd = new Product { ProductId = 1, ProductName = "Banana", ProductQuantity = 25, ProductPrice = 1, ProductDescription = "Potassium", ProductImage = ""};

            _fixture.context.UpdateProduct(newProd);
            var result = _fixture.context.GetProductById(newProd.ProductId);

            Assert.Equal(newProd.ProductQuantity, result.Result.ProductQuantity);
        }

        [Fact]
        public void UpdateProduct_ThrowsExceptionWhenProductNotInDb()
        {
            Product newProd = new Product(5, "Orange", 1, 4, "Orange you gonna ask why I didn't use banana?", "");

            Action badAction = () => _fixture.context.UpdateProduct(newProd);

            Assert.Throws<DbUpdateConcurrencyException>(badAction);
        }

        [Fact]
        public void GetUserById_RetrievesGivenUser()
        {
            var result = _fixture.context.GetUserById(1);

            Assert.Equal("John", result.Result.firstName);
        }

        [Fact]
        public void GetUserById_ReturnsNullWhenRequestedUserNotInDb()
        {
            var result = _fixture.context.GetUserById(50);

            Assert.Null(result.Result);
        }

        [Fact]
        public void CreateNewUser_ReturnsTrueWhenItSuccessfullyCreatesUserWithNewEmail()
        {
            User newUser = new User("Kim", "Buckley", "kim@kime.com", "buck6");

            var result = _fixture.context.CreateNewUser(newUser);

            Assert.True(result.Result);
        }

        [Fact]
        public void CreateNewUser_ReturnsFalseIfItTriesToCreateUserWithExistingEmail()
        {
            User newUser = new User("John", "Doe", "test@gmail.com", "pass");

            var result = _fixture.context.CreateNewUser(newUser);

            Assert.False(result.Result);
        }

        [Fact]
        public void GetUserByEmailAndPassword_SuccessfullyFindsRequestedUserWithProperEmailAndPassword()
        {
            string givenEmail = "test@gmail.com";
            string givenPass = "pass";

            var result = _fixture.context.GetUserByEmailAndPassword(givenEmail, givenPass);

            Assert.IsType<User>(result.Result);
            Assert.Equal("John", result.Result.firstName);
        }

        [Fact]
        public void GetUserByEmailAndPassword_ReturnsNullWhenOnlyProvidedProperEmailAndNotPassword()
        {
            string givenEmail = "test@gmail.com";
            string givenPass = "fail";

            var result = _fixture.context.GetUserByEmailAndPassword(givenEmail, givenPass);

            Assert.Null(result.Result);
        }

        [Fact]
        public void GetUserByEmailAndPassword_ReturnsNullWhenOnlyProvidedProperPasswordAndNotEmail()
        {
            string givenEmail = "invalid@gmail.com";
            string givenPass = "pass";

            var result = _fixture.context.GetUserByEmailAndPassword(givenEmail, givenPass);

            Assert.Null(result.Result);
        }

        [Fact]
        public void GetUserByEmailAndPassword_ReturnsNullWhenProvidedNeitherProperEmailNorPassword()
        {
            string givenEmail = "invalid@gmail.com";
            string givenPass = "fail";

            var result = _fixture.context.GetUserByEmailAndPassword(givenEmail, givenPass);

            Assert.Null(result.Result);
        }

        [Fact]
        public void UpdateUserPassword_UpdatesPasswordProperly()
        {
            string givenEmail = "test@gmail.com";
            string newPass = "pass1";

            var result = _fixture.context.UpdateUserPassword(newPass, givenEmail);

            Assert.IsType<User>(result.Result);
            Assert.Equal(newPass, result.Result.password);
        }

        [Fact]
        public void UpdateUserPassword_ReturnsNullWhenEmailNotFound()
        {
            string givenEmail = "thisWill@fail.com";
            string newPass = "pass1";

            var result = _fixture.context.UpdateUserPassword(newPass, givenEmail);

            Assert.Null(result.Result);
        }

        [Fact]
        public void UpdateUserPassword_ReturnsNullWhenEmailIsNotDifferent()
        {
            string givenEmail = "test@gmail.com";
            string oldPass = "pass";

            var result = _fixture.context.UpdateUserPassword(oldPass, givenEmail);

            Assert.Null(result.Result);
        }
    }
}
