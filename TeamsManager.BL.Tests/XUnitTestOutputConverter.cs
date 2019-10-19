using System.IO;
using System.Text;
using Xunit.Abstractions;

namespace TeamsManager.BL.Tests
{
    internal class XUnitTestOutputConverter : TextWriter
    {
        private readonly ITestOutputHelper output;
        public XUnitTestOutputConverter(ITestOutputHelper output) => this.output = output;
        public override Encoding Encoding => Encoding.UTF8;

        public override void WriteLine(string message)
        {
            output.WriteLine(message);
        }
        public override void WriteLine(string format, params object[] args)
        {
            output.WriteLine(format, args);
        }
    }
}
