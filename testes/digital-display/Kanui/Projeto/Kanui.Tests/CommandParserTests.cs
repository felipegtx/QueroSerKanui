using Kanui.DI;
using Kanui.IO.Abstractions;
using Kanui.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Kanui.Tests.Fakes;

namespace Kanui.Tests
{
    [TestClass]
    public class CommandParserTests
    {
        [TestMethod]
        public void Validate_Input_Parameters()
        {
            /// WE first setup the fake for IO bound operation
            InstanceResolverFor<IFSController>.InstanceBuilder = () => new FakeFSController { FakeExists = _ => !string.IsNullOrEmpty(_) };
            InstanceResolverFor<ILogOutput>.InstanceBuilder = () => new FakeLogOutput();

            /// Now based on the following scenarios [key=parameter, value=is it expected to be valid]
            var parameterDictionary = new Dictionary<string, bool> 
            {
                {"i>C:\\MeuArquivo.txt", true},
                {"t>C:\\MeuArquivo.txt", true},
                {"C:\\MeuArquivo.txt", false},
                {"t", false},
                {"i>", false},
                {">C:\\MeuArquivo.txt", false},
                {">>C:\\MeuArquivo.txt", false}
            };

            /// We check the odds
            foreach (var parameter in parameterDictionary)
            {
                var result = CommandParser.Parse(parameter.Key);
                Assert.AreEqual(result.IsValid, parameter.Value);
            }
        }
    }
}
