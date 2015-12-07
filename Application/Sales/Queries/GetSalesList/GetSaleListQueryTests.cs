﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMoq;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Common.Extensions;
using CleanArchitecture.Domain.Customers;
using CleanArchitecture.Domain.Employees;
using CleanArchitecture.Domain.Products;
using CleanArchitecture.Domain.Sales;
using NUnit.Framework;

namespace CleanArchitecture.Application.Sales.Queries.GetSalesList
{
    [TestFixture]
    public class GetSaleListQueryTests
    {
        private GetSalesListQuery _query;
        private AutoMoqer _mocker;
        private Sale _sale;

        private const int SaleId = 1;
        private static readonly DateTime Date = new DateTime(2001, 2, 3);
        private const string CustomerName = "Customer 1";
        private const string EmployeeName = "Employee 1";
        private const string ProductName = "Product 1";
        private const decimal UnitPrice = 1.23m;
        private const int Quantity = 2;
        private const decimal TotalPrice = 2.46m;

        [SetUp]
        public void SetUp()
        {
            var customer = new Customer
            {
                Name = CustomerName
            };

            var employee = new Employee
            {
                Name = EmployeeName
            };

            var product = new Product
            {
                Name = ProductName
            };

            _sale = new Sale()
            {
                Id = SaleId,
                Date = Date,
                Customer = customer,
                Employee = employee,
                Product = product,
                UnitPrice = UnitPrice,
                Quantity = Quantity,
                TotalPrice = TotalPrice
            };

            _mocker = new AutoMoqer();

            _query = _mocker.Create<GetSalesListQuery>();
        }

        [Test]
        public void TestExecuteShouldReturnListOfSales()
        {
            _mocker.GetMock<IDbSet<Sale>>()
                .SetUpDbSet(new List<Sale> { _sale });

            _mocker.GetMock<IDatabaseContext>()
                .Setup(p => p.Sales)
                .Returns(_mocker.GetMock<IDbSet<Sale>>().Object);

            var results = _query.Execute();

            var result = results.Single();

            Assert.That(result.Id, Is.EqualTo(SaleId));
            Assert.That(result.Date, Is.EqualTo(Date));
            Assert.That(result.CustomerName, Is.EqualTo(CustomerName));
            Assert.That(result.EmployeeName, Is.EqualTo(EmployeeName));
            Assert.That(result.ProductName, Is.EqualTo(ProductName));
            Assert.That(result.UnitPrice, Is.EqualTo(UnitPrice));
            Assert.That(result.Quantity, Is.EqualTo(Quantity));
            Assert.That(result.TotalPrice, Is.EqualTo(TotalPrice));
        }
    }    
}