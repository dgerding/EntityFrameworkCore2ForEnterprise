﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnLineStore.Common;
using OnLineStore.Core.BusinessLayer.Requests;
using OnLineStore.Core.BusinessLayer.Responses;
using OnLineStore.Core.DataLayer.DataContracts;
using OnLineStore.Core.EntityLayer.Sales;
using OnLineStore.WebAPI.Controllers;
using OnLineStore.WebAPI.Requests;
using Xunit;

namespace OnLineStore.WebAPI.UnitTests
{
    public class SalesControllerTests
    {
        [Fact]
        public async Task TestGetOrdersAsync()
        {
            // Arrange
            var logger = LoggingHelper.GetLogger<SalesController>();
            var salesService = ServiceMocker.GetSalesService(nameof(TestGetOrdersAsync));
            var controller = new SalesController(logger, salesService);

            // Act
            var response = await controller.GetOrdersAsync() as ObjectResult;
            var value = response.Value as IPagedResponse<OrderInfo>;

            salesService.Dispose();

            // Assert
            Assert.False(value.DidError);
        }

        [Fact]
        public async Task TestGetOrdersByCurrencyAsync()
        {
            // Arrange
            var logger = LoggingHelper.GetLogger<SalesController>();
            var salesService = ServiceMocker.GetSalesService(nameof(TestGetOrdersByCurrencyAsync));
            var controller = new SalesController(logger, salesService);
            var currencyID = (short?)1000;

            // Act
            var response = await controller.GetOrdersAsync(currencyID: currencyID) as ObjectResult;
            var value = response.Value as IPagedResponse<OrderInfo>;

            salesService.Dispose();

            // Assert
            Assert.False(value.DidError);
            Assert.True(value.Model.Count() > 0);
            Assert.True(value.Model.Where(item => item.CurrencyID == currencyID).Count() == value.Model.Count());
        }

        [Fact]
        public async Task TestGetOrdersByCustomerAsync()
        {
            // Arrange
            var logger = LoggingHelper.GetLogger<SalesController>();
            var salesService = ServiceMocker.GetSalesService(nameof(TestGetOrdersByCustomerAsync));
            var controller = new SalesController(logger, salesService);
            var customerID = 1;

            // Act
            var response = await controller.GetOrdersAsync(customerID: customerID) as ObjectResult;
            var value = response.Value as IPagedResponse<OrderInfo>;

            salesService.Dispose();

            // Assert
            Assert.False(value.DidError);
            Assert.True(value.Model.Where(item => item.CustomerID == customerID).Count() == value.Model.Count());
        }

        [Fact]
        public async Task TestGetOrdersByEmployeeAsync()
        {
            // Arrange
            var logger = LoggingHelper.GetLogger<SalesController>();
            var salesService = ServiceMocker.GetSalesService(nameof(TestGetOrdersByEmployeeAsync));
            var controller = new SalesController(logger, salesService);
            var employeeID = 1;

            // Act
            var response = await controller.GetOrdersAsync(employeeID: employeeID) as ObjectResult;
            var value = response.Value as IPagedResponse<OrderInfo>;

            salesService.Dispose();

            // Assert
            Assert.False(value.DidError);
            Assert.True(value.Model.Where(item => item.EmployeeID == employeeID).Count() == value.Model.Count());
        }

        [Fact]
        public async Task TestGetOrderAsync()
        {
            // Arrange
            var logger = LoggingHelper.GetLogger<SalesController>();
            var salesService = ServiceMocker.GetSalesService(nameof(TestGetOrderAsync));
            var controller = new SalesController(logger, salesService);
            var id = 1;

            // Act
            var response = await controller.GetOrderAsync(id) as ObjectResult;
            var value = response.Value as ISingleResponse<Order>;

            salesService.Dispose();

            // Assert
            Assert.False(value.DidError);
        }

        [Fact]
        public async Task TestGetNonExistingOrderAsync()
        {
            // Arrange
            var logger = LoggingHelper.GetLogger<SalesController>();
            var salesService = ServiceMocker.GetSalesService(nameof(TestGetNonExistingOrderAsync));
            var controller = new SalesController(logger, salesService);
            var id = 0;

            // Act
            var response = await controller.GetOrderAsync(id) as ObjectResult;
            var value = response.Value as ISingleResponse<Order>;

            salesService.Dispose();

            // Assert
            Assert.False(value.DidError);
        }

        [Fact]
        public async Task TestGetCreateOrderRequestAsync()
        {
            // Arrange
            var logger = LoggingHelper.GetLogger<SalesController>();
            var salesService = ServiceMocker.GetSalesService(nameof(TestGetCreateOrderRequestAsync));
            var controller = new SalesController(logger, salesService);

            // Act
            var response = await controller.GetCreateOrderRequestAsync() as ObjectResult;
            var value = response.Value as ISingleResponse<CreateOrderRequest>;

            salesService.Dispose();

            // Assert
            Assert.False(value.DidError);
            Assert.True(value.Model.Products.Count() > 0);
            Assert.True(value.Model.Customers.Count() > 0);
        }

        [Fact]
        public async Task TestCreateOrderAsync()
        {
            // Arrange
            var logger = LoggingHelper.GetLogger<SalesController>();
            var salesService = ServiceMocker.GetSalesService(nameof(TestCreateOrderAsync));
            var controller = new SalesController(logger, salesService);
            var model = new OrderRequest
            {
                CustomerID = 1,
                PaymentMethodID = new Guid("7671A4F7-A735-4CB7-AAB4-CF47AE20171D"),
                Comments = "Order from unit tests",
                CreationUser = "unitests",
                CreationDateTime = DateTime.Now,
                Details = new List<OrderDetailRequest>
                {
                    new OrderDetailRequest
                    {
                        ProductID = 1,
                        ProductName = "The King of Fighters XIV",
                        Quantity = 1,
                    }
                }
            };

            // Act
            var response = await controller.CreateOrderAsync(model) as ObjectResult;
            var value = response.Value as ISingleResponse<Order>;

            salesService.Dispose();

            // Assert
            Assert.False(value.DidError);
            Assert.True(value.Model.OrderID.HasValue);
        }

        [Fact]
        public async Task TestCloneOrderAsync()
        {
            // Arrange
            var logger = LoggingHelper.GetLogger<SalesController>();
            var salesService = ServiceMocker.GetSalesService(nameof(TestCloneOrderAsync));
            var controller = new SalesController(logger, salesService);
            var id = 1;

            // Act
            var response = await controller.CloneOrderAsync(id) as ObjectResult;
            var value = response.Value as ISingleResponse<Order>;

            salesService.Dispose();

            // Assert
            Assert.False(value.DidError);
        }
    }
}
