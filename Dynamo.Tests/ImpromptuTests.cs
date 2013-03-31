using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImpromptuInterface;
using ImpromptuInterface.Dynamic;

namespace Dynamo.Tests
{   [TestFixture]
    public class ImpromptuTests
    {
        [Test]
        public void Should_create_dynamic_factory()
        {
            var x = new ImpromptuFactory();
            var factory = Builder.New();
            var pen = factory.Object(Name: "pen");

            Assert.That(pen.ActLike<IProduct>().Name == "pen");
        }

        [Test]
        public void Should()
        {
            dynamic New = Builder.New();

            var product = New.Product(
                    Name: "pen",
                    Price: 14.95
                );

            var otherPen = Build.NewObject(
                Name: "pen"
            );
            
            Assert.That(otherPen.Name == "pen");
        }

        [Test]
        public void Should_be_able_to_impromptu_class_instance()
        {
            // try to take an object, make it dynamic, then do crazy shit, but then arrive back at the class, none the wiser

            var product = new Product { Name = "Pen" };
            dynamic dProduct = product;
            dProduct.Buy = Return<bool>.Arguments<string, double>((prod, quantity) =>
            {
                dProduct.Profit = 10 * quantity;
                Console.Write(string.Format("Order placed for {0} of {1}", quantity, prod));
                return true;
            });

            var order = Impromptu.ActLike<IOrderable>(dProduct);
            var orderedProduct = Impromptu.ActLike<IProduct>(dProduct);

            Assert.That(orderedProduct.Name == "Pen");
        }
    }

public interface IProduct
{
    string Name { get; set; }
}

public class Product : IProduct
{
    public string Name { get; set; }
}

public interface IOrderable
{
    void Buy();
    double Profit { get; set; }
}
}
