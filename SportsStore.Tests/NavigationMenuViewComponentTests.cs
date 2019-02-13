using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using Moq;
using SportsStore.Components;
using SportsStore.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SportsStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void Can_Select_Categories()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product { ProductID = 1, Name = "P1", Category = "Jabłka" },
                new Product { ProductID = 2, Name = "P2", Category = "Jabłka" },
                new Product { ProductID = 3, Name = "P3", Category = "Śliwki" },
                new Product { ProductID = 4, Name = "P4", Category = "Pomarańcze" }
            }).AsQueryable<Product>);

            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);

            string[] result = ((IEnumerable<string>)(target.Invoke() as ViewViewComponentResult).ViewData.Model).ToArray();

            Assert.True(Enumerable.SequenceEqual(new string[] { "Jabłka", "Pomarańcze", "Śliwki" }, result));
        }

        [Fact]
        public void Indicates_Selected_Category()
        {
            string categoryToSelect = "Jabłka";
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product { ProductID = 1, Name = "P1", Category = "Jabłka" },
                new Product { ProductID = 4, Name = "P2", Category = "Pomarańcze" }
            }).AsQueryable<Product>);

            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);
            target.ViewComponentContext = new ViewComponentContext
            {
                ViewContext = new ViewContext
                {
                    RouteData = new RouteData()
                }
            };

            target.RouteData.Values["category"] = categoryToSelect;

            string result = (string)(target.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"];

            Assert.Equal(categoryToSelect, result);
        }
    }
}