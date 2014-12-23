using Kanui.DI;
using Kanui.IO.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kanui.Parsers
{
    public class DataParserResult
    {
        public const string DATA_DICTIONARY_FILE_NAME = "map.kanui";
        public const string INVALID_LINE_MESSAGE = @"/!\\erro de formato/!\";
        public IEnumerable<string> Result { get; private set; }
        public string PathToHashFile { get; private set; }

        private DataParserResult(CommandParser commandParser)
        {
            this.PathToHashFile = string.Format(@"{0}\{1}",
                   Environment.CurrentDirectory,
                   DATA_DICTIONARY_FILE_NAME);

            this.Result = this.TryLoadUsing(commandParser);
        }

        private IEnumerable<string> TryLoadUsing(CommandParser commandParser)
        {
            if ((commandParser != null) && (commandParser.IsValid))
            {
                switch (commandParser.Command)
                {
                    case CommandParser.ParserCommand.Identify:
                        return this.TryIdentifyDataFrom(commandParser.PathToFile);
                    case CommandParser.ParserCommand.Trainning:
                        return this.RebuildTrainingDataUsign(commandParser.PathToFile);
                    default:
                        throw new InvalidOperationException("Invalid command");
                }
            }
            return new string[] { };
        }

        private IEnumerable<string> RebuildTrainingDataUsign(string pathToFile)
        {
            IEnumerable<string> fileContent;

            /// We first gather and do some basic validation on the file data
            if (InstanceResolverFor<IFSController>.Instance.TryRead(pathToFile, out fileContent) &&
                this.IsValidFile(fileContent))
            {
                Dictionary<int, int> result = new Dictionary<int, int>();

                /// Then we need to check if the group we collected contains a valid Y axis structure
                if ((fileContent.Count() != 4) /// The training file must have only one set of digits with 4 rows
                    || (from line in fileContent.Take(3) /// We can avoid the 4th empty line  
                        where line.Length != 30
                        select true).FirstOrDefault()) { yield return INVALID_LINE_MESSAGE; }
                else
                {
                    /// If so, we move on calculating a hash value for each digit so we can easily identify them further on
                    int currentItemIndex;
                    int hashCollidedOn;

                    for (int heigth = 0; heigth < fileContent.Count(); heigth += 4)
                    {
                        foreach (var computedResult in Compute(

                            /// The subset we're working on...
                            (from linePortion in fileContent.Skip(heigth).Take(3) select linePortion).ToArray(),

                            /// How we're supposed to handle the items we're reading
                            (acumulator, x) =>
                            {
                                currentItemIndex = x / 3;
                                if (result.TryGetValue(acumulator, out hashCollidedOn))
                                {
                                    /// Here we would handle hash collisions - not important at this time. 
                                    throw new InvalidOperationException(string.Format("The calculated hash ({0}) for '{1}' collided with the calculated hash for '{2}",
                                        acumulator, currentItemIndex, hashCollidedOn));
                                }
                                else
                                {
                                    result.Add(acumulator, currentItemIndex);
                                    return string.Format("{0} - {1}", currentItemIndex, acumulator);
                                }
                            }, 30))
                        {
                            /// The computed result is now safe to be returned
                            yield return computedResult;
                        }
                    }

                    InstanceResolverFor<ISerializer>.Instance.Serialize(result, this.PathToHashFile);
                }
            }
        }

        private IEnumerable<string> TryIdentifyDataFrom(string pathToFile)
        {
            IEnumerable<string> fileContent;

            /// We first gather and do some basic validation on the file data
            if (InstanceResolverFor<IFSController>.Instance.TryRead(pathToFile, out fileContent) &&
                this.IsValidFile(fileContent))
            {

                /// We need the latest version on the hash file
                var hashedData = InstanceResolverFor<ISerializer>.Instance.DeserializeFrom<Dictionary<int, int>>(this.PathToHashFile);
                if (hashedData == null) { throw new Exception(string.Format("Hash file was not found at '{0}'.", this.PathToHashFile)); }

                /// Line by line the result stream
                StringBuilder resultStream = new StringBuilder();

                /// If so, we move on calculating a hash value for each digit so we can easily identify them further on
                int correspondingItem;
                for (int heigth = 0; heigth < fileContent.Count(); heigth += 4)
                {
                    resultStream.Clear();
                    foreach (var computedResult in Compute(

                        /// The subset we're working on...
                        (from linePortion in fileContent.Skip(heigth).Take(3) select linePortion).ToArray(),

                        /// How we're supposed to handle the items we're reading
                        (acumulator, x) =>
                        {
                            var hash = acumulator;
                            if (hashedData.TryGetValue(hash, out correspondingItem))
                            {
                                return correspondingItem.ToString();
                            }
                            else
                            {
                                return INVALID_LINE_MESSAGE;
                            }
                        }))
                    {

                        if (computedResult == INVALID_LINE_MESSAGE)
                        {
                            yield return INVALID_LINE_MESSAGE;
                            break;
                        }
                        else
                        {
                            /// The computed result is now safe to be returned
                            resultStream.Append(computedResult);
                        }
                    }

                    if (resultStream.Length > 0) { yield return resultStream.ToString(); }
                }
            }
        }

        private IEnumerable<string> Compute(string[] wokingLineContent, Func<int, int, string> validator, int uBound = 27)
        {
            int acumulator = 0;
            string result;
            for (int x = 0; x <= uBound; x++)
            {
                if ((x > 0) && ((x % 3) == 0))
                {
                    yield return result = validator(acumulator, x - 3);

                    acumulator = 0;

                    if (result == INVALID_LINE_MESSAGE)
                    {
                        yield break;
                    }
                }

                if (x < uBound)
                {
                    var xRef = x % 3;
                    for (int y = 0; y < 3; y++)
                    {
                        if (wokingLineContent[y].Length != uBound)
                        {
                            yield return INVALID_LINE_MESSAGE;
                        }
                        else
                        {
                            var d = ComputeValueFor(wokingLineContent[y][x]);
                            acumulator += (((y ^ d) + (xRef ^ d)) / 3) + (d * y);
                        }
                    }
                }
            }
            yield break;
        }

        private int ComputeValueFor(char digitRange)
        {
            return digitRange.GetHashCode();
        }
        
        private bool IsValidFile(IEnumerable<string> fileContent)
        {
            return (fileContent != null) && ((fileContent.Count() % 4) == 0);
        }

        public static DataParserResult LoadUsing(CommandParser commandParser)
        {
            return new DataParserResult(commandParser);
        }
    }
}
