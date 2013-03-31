using Microsoft.Scripting.Hosting;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using ImpromptuInterface;
using System.IO;

namespace Dynamo
{
    public class Dynamo
    {
        public ScriptEngine Engine { get; set; }
        public ScriptRuntime Runtime { get; set; }

        /// <summary>
        /// The ruby factory object responsible for constucting the ruby types
        /// bootstrapped using the rubyhost
        /// </summary>
        /// <example>
        /// Factory.SetVariable("Sum", 15);
        /// </example>
        public ScriptScope Factory { get; set; }

        public Dynamo()
        {
            Engine = IronRuby.Ruby.CreateEngine();
            //Runtime = IronRuby.Ruby.CreateRuntime(new ScriptRuntimeSetup { DebugMode = true });
            Runtime = IronRuby.Ruby.CreateRuntime();

            var paths = Engine.GetSearchPaths().ToList();
            paths.Add(@"C:\\Program Files (x86)\\IronRuby 1.1\\Lib\\ruby\\1.9.1");
            paths.Add(@"C:\\Program Files (x86)\\IronRuby 1.1\\Lib\\ironruby");
            paths.Add(Assembly.GetExecutingAssembly().Location);
            Engine.SetSearchPaths(paths);


        }

        protected T Execute<T>(string file)
        {
            var source = Engine.CreateScriptSourceFromFile(file);
            source.Compile(new ReportingErrorListener(Console.Out));
            return source.Execute<T>();
        }

        public dynamic ExecuteRubyHost()
        {
            // where ever you drop dynamo the rubyhost should bootstrap any ruby that it finds in the same location
            var files = Directory.EnumerateFiles(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location))
                                 .Where(x => x.EndsWith(".rb"));

            foreach (var rb in files)
            {
                Engine.ExecuteFile(rb);
            }
            var source = Engine.CreateScriptSourceFromFile("rubyhost.rb");
            source.Compile(new ReportingErrorListener(Console.Out));
            
            Factory = Runtime.Globals;
            
            return source.Execute();
        }

        public IEnumerable<string> GetInstanceNames()
        {
            return Engine.Runtime.Globals.GetVariableNames();
        }

        /// <summary>
        /// Indicates if the rubyhost has the class in scope.
        /// Is the file dependency included, is it required etc.
        /// </summary>
        /// <param name="className">The class name of the ruby object</param>
        /// <returns></returns>
        public bool CanInstantiate(string className)
        {
            return Engine.Runtime.Globals.ContainsVariable(className);
        }

        public dynamic GetInstance(string instanceName)
        {
            dynamic instanceVariable;
            var instanceVariableResult = Engine.Runtime.Globals.TryGetVariable(instanceName, out instanceVariable);

            if (!instanceVariableResult && instanceVariable == null)
                throw new InvalidOperationException(string.Format("Unable to find {0}", instanceName));

            dynamic instance = Engine.Operations.CreateInstance(instanceVariable);
            return instance;
        }

        /// <summary>
        /// Gets a typed instance of a Ruby declared type
        /// </summary>
        /// <typeparam name="T">The .Net interface for the type instance being retrieved.</typeparam>
        /// <param name="instanceName">The name of the Ruby instance being bridged across to .Net</param>
        /// <returns>An instance matching the type given from Ruby as a .Net interface type instance.</returns>
        public T GetInstance<T>(string instanceName) where T : class
        {
            dynamic instanceVariable;
            var instanceVariableResult = Engine.Runtime.Globals.TryGetVariable(instanceName, out instanceVariable);

            if (!instanceVariableResult && instanceVariable == null)
                throw new InvalidOperationException(string.Format("Unable to find {0}", instanceName));

            dynamic instance = Engine.Operations.CreateInstance(instanceVariable);
            return Impromptu.ActLike<T>(instance);
        }
    }
}
