using Kanui.IO.Abstractions;
using System;
using System.Collections.Generic;

namespace Kanui.Tests.Fakes
{
    internal class FakeFSController : IFSController
    {
        public Func<string, bool> FakeExists { get; set; }
        public Func<string, IEnumerable<string>> FakeTryRead { get; set; }

        bool IFSController.TryRead(string pathToFile, out IEnumerable<string> fileContent)
        {
            return ((fileContent = this.FakeTryRead(pathToFile)) != null);
        }

        bool IFSController.Exists(string pathToFile)
        {
            return FakeExists(pathToFile);
        }
    }
}
