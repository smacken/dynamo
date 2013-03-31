using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamo
{
    // ** Usage **
    //var engine = IronRuby.Ruby.CreateEngine();
    //engine.SetSearchPaths(SearchPaths);
    //var source = engine.CreateScriptSourceFromFile(fileName);
    //source.Compile(new ReportingErrorListener(Console.Out));
    //var result = source.Execute<T>();
    public class ReportingErrorListener : ErrorListener
    {
        private readonly TextWriter _writer;

        public ReportingErrorListener(TextWriter writer)
        {
            _writer = writer;
        }

        public override void ErrorReported(
            ScriptSource source, string message,
            SourceSpan span, int errorCode,
            Severity severity)
        {
            _writer.WriteLine("Error starting at line {0} column {1}",
                               span.Start.Line, span.Start.Column);
            _writer.WriteLine(message);
        }
    }
}
