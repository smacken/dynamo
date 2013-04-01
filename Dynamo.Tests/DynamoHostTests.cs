using ImpromptuInterface;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamo.Tests
{
    [TestFixture]
    public class DynamoHostTests
    {
        public DynamoHost Host { get; set; }

        [TestFixtureSetUp]
        public void Setup()
        {
            Host = new DynamoHost();
        }

        [Test]
        public void Can_create_dynamic_object_without_class()
        {
            dynamic product = Host.New.Object(Name: "Pen", Price: 14.95);

            new ClassThatTakesProduct().Order(Impromptu.ActLike<IProduct>(product), 12); 
        }
    }

    public class ClassThatTakesProduct
    {
        public void Order(IProduct product, int quantity)
        {
            Assert.Pass();
        }
    }

    public class OrderRepository
    {
        public IProduct Product { get; set; }
    }
}
