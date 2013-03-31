using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dynamo;

namespace Dynamo.Tests
{
    [TestFixture]
    public class DynamoTests
    {
        [Test]
        public void Can_create_dynamo_instance()
        {
            var dynamo = new Dynamo();
            //dynamo.Should(Be.Not.Null);
            Assert.That(dynamo != null);
        }

        [Test]
        public void Can_query_bridged_types()
        {
            var dynamo = new Dynamo();
            dynamo.ExecuteRubyHost();
            var types = dynamo.GetInstanceNames();

            foreach (var bridge in types)
            {
                Console.Write(bridge);
            }

            Assert.That(types.Count() > 0);
        }

        [Test]
        public void Can_create_ruby_instance()
        {
            var dynamo = new Dynamo();
            dynamo.ExecuteRubyHost();
            IPerson person = dynamo.GetInstance<IPerson>("Person");
            person.greet();
            Assert.That(person != null);
        }
    }

    public interface IPerson
    {
        void greet();
    }
}
