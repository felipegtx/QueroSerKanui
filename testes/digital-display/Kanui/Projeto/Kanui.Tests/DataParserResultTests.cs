using Kanui.DI;
using Kanui.IO.Abstractions;
using Kanui.Parsers;
using Kanui.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Kanui.Tests
{
    [TestClass]
    public class DataParserResultTests
    {
        [TestMethod]
        public void Validate_Command_Received()
        {
            /// Here we define the file contents into lines
            var dataSourceContents = string.Format(" _     _  _     _  _  _  _  _ {0}| |  | _| _||_||_ |_   ||_||_|{0}|_|  ||_  _|  | _||_|  ||_| _|{0}", ":").Split(':');
            var validateContents = string.Format("    _  _     _  _  _  _  _ {0}  | _| _||_||_ |_   ||_||_|{0}  ||_  _|  | _||_|  ||_| _|{0}{0} _  _     _     _  _  _  _ {0}|_  _|  ||_||_| _||_   ||_|{0}|_||_   | _|  | _| _|  ||_|{0}{0} _  _  _  _  _  _  _  _  _ {0}| || || || || || || || || |{0}|_||_||_||_||_||_||_||_||_|{0}{0}                           {0}  |  |  |  |  |  |  |  |  |{0}  |  |  |  |  |  |  |  |  |{0}{0} _  _  _  _  _  _  _  _  _ {0} _| _| _| _| _| _| _| _| _|{0}|_ |_ |_ |_ |_ |_ |_ |_ |_ {0}{0} _  _  _  _  _  _  _  _  _ {0} _| _| _| _| _| _| _| _| _|{0} _| _| _| _| _| _| _| _| _|{0}{0}                           {0}|_||_||_||_||_||_||_||_||_|{0}  |  |  |  |  |  |  |  |  |{0}{0} _  _  _  _  _  _  _  _  _ {0}|_ |_ |_ |_ |_ |_ |_ |_ |_ {0} _| _| _| _| _| _| _| _| _|{0}{0} _  _  _  _  _  _  _  _  _ {0}|_ |_ |_ |_ |_ |_ |_ |_ |_ {0}|_||_||_||_||_||_||_||_||_|{0}{0} _  _  _  _  _  _  _  _  _ {0}  |  |  |  |  |  |  |  |  |{0}  |  |  |  |  |  |  |  |  |{0}{0} _  _  _  _  _  _  _  _  _ {0}|_||_||_||_||_||_||_||_||_|{0}|_||_||_||_||_||_||_||_||_|{0}{0} _  _  _  _  _  _  _  _  _ {0}|_||_||_||_||_||_||_||_||_|{0} _| _| _| _| _| _| _| _| _|{0}{0}    _  _     _  _  _  _  _{0}  | _| _||_||_ |_   ||_||_|{0}  ||_  _|  | _||_|  ||_| _| {0}{0} _  _  _  _  _  _  _  _    {0}| || || || || || || ||_   |{0}|_||_||_||_||_||_||_| _|  |{0}{0}    _  _  _  _  _  _     _ {0}|_||_|| || ||_   |  |  | _|{0}  | _||_||_||_|  |  |  | _|{0}{0}    _  _     _  _  _  _  _ {0}  | _| _||_||_ |_   ||_||_|{0}  ||_  _|  | _||_|  ||_| _|{0}{0}                           {0}  |  |  |  |  |  |  |  |  |{0}  |  |  |  |  |  |  |  |  |{0}{0} _  _  _  _  _  _  _  _  _ {0}  |  |  |  |  |  |  |  |  |{0}  |  |  |  |  |  |  |  |  |{0}{0} _  _  _  _  _  _  _  _  _ {0} _|| || || || || || || || |{0}|_ |_||_||_||_||_||_||_||_|{0}{0} _  _  _  _  _  _  _  _  _ {0} _| _| _| _| _| _| _| _| _|{0} _| _| _| _| _| _| _| _| _|{0}{0} _  _  _  _  _  _  _  _  _ {0}|_||_||_||_||_||_||_||_||_|{0}|_||_||_||_||_||_||_||_||_|{0}{0} _  _  _  _  _  _  _  _  _ {0}|_ |_ |_ |_ |_ |_ |_ |_ |_ {0} _| _| _| _| _| _| _| _| _|{0}{0} _  _  _  _  _  _  _  _  _ {0}|_ |_ |_ |_ |_ |_ |_ |_ |_ {0}|_||_||_||_||_||_||_||_||_|{0}{0} _  _  _  _  _  _  _  _  _ {0}|_||_||_||_||_||_||_||_||_|{0} _| _| _| _| _| _| _| _| _|{0}{0}    _  _  _  _  _  _     _ {0}|_||_|| || ||_   |  |  ||_ {0}  | _||_||_||_|  |  |  | _|{0}{0}    _  _     _  _  _  _  _ {0}|_| _| _||_||_ |_   ||_||_|{0}  ||_  _|  | _||_|  ||_| _|{0}{0} _  _  _  _  _  _  _  _    {0}| || || || || || || ||_   |{0}|_||_||_||_||_||_||_| _|  |{0}{0}    _  _  _  _  _  _     _ {0}|_||_|| ||_||_   |  |  | _|{0}  | _||_||_||_|  |  |  | _|{0}{0} _  _  _  _  _  _  _  _  _ {0}| || || || || || ||_ |_ |_ {0}|_||_||_||_||_||_||_||_||_|{0}{0} _  _  _     _  _  _  _  _ {0}| ||_ | |  || || || ||_ |_ {0}|_||_||_|  ||_||_||_||_||_|{0}", ":").Split(':');
            Dictionary<int, int> serializedHash = null;

            /// WE first setup the fakes we'll need
            InstanceResolverFor<ILogOutput>.InstanceBuilder = () => new FakeLogOutput();
            InstanceResolverFor<IFSController>.InstanceBuilder = () => new FakeFSController
            {
                FakeExists = _ => true,
                FakeTryRead = _ => _.Contains("source") ? dataSourceContents : validateContents
            };
            InstanceResolverFor<ISerializer>.InstanceBuilder = () => new FakeSerializer
            {
                FakeDeserialize = () => serializedHash,
                FakeSerialize = _ => serializedHash = _
            };

            /// Now based on the following scenarios [key=parameter, value=parameter type]
            var parameterDictionary = new Dictionary<string, CommandParser.ParserCommand> 
            {
                {"t>C:\\source.txt", CommandParser.ParserCommand.Trainning},
                {"i>C:\\MeuArquivo.txt", CommandParser.ParserCommand.Identify}
            };

            foreach (var parameter in parameterDictionary)
            {
                /// We need to gather an instance for CommandParser
                var command = CommandParser.Parse(parameter.Key);

                /// Some extra cautious checks (never enought right? - better safe than sorry)
                Assert.AreEqual(command.IsValid, true);
                Assert.AreEqual(command.Command, parameter.Value);

                /// Now we're talking
                var result = DataParserResult.LoadUsing(command);
                Assert.IsNotNull(result);
                if (command.Command == CommandParser.ParserCommand.Trainning)
                {
                    Assert.AreEqual(10, result.Result.Count());
                    Assert.AreEqual(serializedHash.Count, result.Result.Count());
                    Assert.IsFalse((from r in result.Result where r == DataParserResult.INVALID_LINE_MESSAGE select true).FirstOrDefault());
                }
                else
                {
                    var errorsFound = (from r in result.Result
                                       where r == DataParserResult.INVALID_LINE_MESSAGE
                                       select 1).Sum();
                    Assert.AreEqual(1, errorsFound);
                    Assert.AreEqual(30, result.Result.Count());
                    var resultArr = result.Result.ToArray();
                    Assert.AreEqual(resultArr[0], "123456789");
                    Assert.AreEqual(resultArr[1], "621943578");
                    Assert.AreEqual(resultArr[2], "000000000");
                    Assert.AreEqual(resultArr[3], "111111111");
                    Assert.AreEqual(resultArr[4], "222222222");
                    Assert.AreEqual(resultArr[5], "333333333");
                    Assert.AreEqual(resultArr[6], "444444444");
                    Assert.AreEqual(resultArr[7], "555555555");
                    Assert.AreEqual(resultArr[8], "666666666");
                    Assert.AreEqual(resultArr[9], "777777777");
                    Assert.AreEqual(resultArr[10], "888888888");
                    Assert.AreEqual(resultArr[11], "999999999");
                    Assert.AreEqual(resultArr[12], DataParserResult.INVALID_LINE_MESSAGE);
                    Assert.AreEqual(resultArr[13], "000000051");
                    Assert.AreEqual(resultArr[14], "490067713");
                    Assert.AreEqual(resultArr[15], "123456789");
                    Assert.AreEqual(resultArr[16], "111111111");
                    Assert.AreEqual(resultArr[17], "777777777");
                    Assert.AreEqual(resultArr[18], "200000000");
                    Assert.AreEqual(resultArr[19], "333333333");
                    Assert.AreEqual(resultArr[20], "888888888");
                    Assert.AreEqual(resultArr[21], "555555555");
                    Assert.AreEqual(resultArr[22], "666666666");
                    Assert.AreEqual(resultArr[23], "999999999");
                    Assert.AreEqual(resultArr[24], "490067715");
                    Assert.AreEqual(resultArr[25], "423456789");
                    Assert.AreEqual(resultArr[26], "000000051");
                    Assert.AreEqual(resultArr[27], "490867713");
                    Assert.AreEqual(resultArr[28], "000000666");
                    Assert.AreEqual(resultArr[29], "060100066");
                }
            }
        }
    }
}
