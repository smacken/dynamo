using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImpromptuInterface;
using ImpromptuInterface.Dynamic;
using System.Dynamic;

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

            var product = Build.NewObject( Name: "Pen" );
            product.Buy = Return<bool>.Arguments<string, double>((prod, quantity) =>
            {
                product.Profit = 10 * quantity;
                Console.Write(string.Format("Order placed for {0} of {1}", quantity, prod));
                return true;
            });

            var order = Impromptu.ActLike<IOrderable>(product);
            var orderedProduct = Impromptu.ActLike<IProduct>(product);

            Assert.That(orderedProduct.Name == "Pen");

            order.Buy("Pen", 3);
            Assert.That(order.Profit == 30);
        }

        [Test]
        public void Should_convert_known_type_to_more_dynamic_type()
        {
            var product = new Product {Name = "Pen" };

            dynamic dProduct = product as dynamic;
            Assert.That(dProduct.Name == "Pen");
            //dynamic dProduct = Impromptu.InvokeConvert(product, typeof(dynamic), true);
        }

        [Test]
        public void Should_append_additional_dynamic_behaviour_once_converted()
        {
            var product = new Product { Name = "Pen" };
            Impromptu.InvokeSet(product, "Profit", default(double));
            dynamic dProduct = product as dynamic;
            var ex = new ExpandoObject();
            
            //dynamic dProduct = Impromptu.CoerceConvert(product, typeof(ImpromptuDictionary));
            //dynamic dProduct = ImpromptuGet.Create(product);
            //dProduct.Profit = default(double);
            

            dProduct.Buy = Return<bool>.Arguments<string, double>((price  , quantity) =>
            {
                Assert.Pass();
                Console.Write(string.Format("Order placed for {0} of {1}", quantity, price));
                return true;
            });

            //IOrderable order = Impromptu.ActLike<IOrderable>(dProduct);
            //order.Buy("19.95", 4);
            
        }

        public void Poco()
        {
            var tPoco = new Product();

            var tSetValue = "1";

            Impromptu.InvokeSet(tPoco, "Prop1", tSetValue);

            Assert.AreEqual(tSetValue, tPoco.Prop1);
        }

    }

public class Reste
{
    public dynamic Cast(object obj, Type castTo)
    {
        return Impromptu.InvokeConvert(obj, castTo, true);
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
    bool Buy(string prod, int quantity);
    double Profit { get; set; }
}
}
