using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImpromptuInterface;
using ImpromptuInterface.Dynamic;

namespace Dynamo
{
    public class DynamoHost
    {
        public Dynamo Dynamo { get; set; }

        /// <summary>
        /// Dynamic object creator
        /// </summary>
        /// <example>
        /// dynamic pen = New.Object(Name: "Pen", Price: 14.95); 
        /// </example>
        public IImpromptuBuilder New { get; set; }

        public DynamoHost()
        {
            Dynamo = new Dynamo();

            New = Builder.New();
        }

        public void WireDynano()
        {
            var conventions = new ConventionBuilder();
            
            foreach( var type in Dynamo.GetInstanceNames())
            {
                conventions.ForType(Type.ReflectionOnlyGetType(type, false, true)).Export();
                //conventions.ForType(typeof(Type)).ExportInterfaces(i => i.IsPublic);
            }
            var config = new ContainerConfiguration().WithDefaultConventions(conventions);
            
            //var container = new CompositionContainer();
        }
    }
}
