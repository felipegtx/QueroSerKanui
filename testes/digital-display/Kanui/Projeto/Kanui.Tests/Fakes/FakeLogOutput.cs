using Kanui.IO.Abstractions;
using System.Diagnostics;

namespace Kanui.Tests.Fakes
{
    public sealed class FakeLogOutput : ILogOutput
    {
        public void Info(string data, params object[] @params)
        {
            Debug.WriteLine(data, @params);
        }
    }
}
