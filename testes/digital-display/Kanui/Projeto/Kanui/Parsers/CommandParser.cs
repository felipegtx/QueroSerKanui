using Kanui.DI;
using Kanui.IO.Abstractions;
using System;

namespace Kanui.Parsers
{
    public sealed class CommandParser
    {
        public const char COMMAND_SEPARATOR = '>';
        public bool IsValid { get; private set; }
        public ParserCommand Command { get; private set; }
        public string PathToFile { get; private set; }

        private CommandParser(string input)
        {
            this.TryParse(input);
        }

        private void TryParse(string input)
        {
            if (string.IsNullOrEmpty(input)) { throw new ArgumentNullException("input"); }

            var data = input.Split(COMMAND_SEPARATOR);
            if (this.IsValid = data.Length == 2)
            {
                char rawCommand;
                ParserCommand command = (ParserCommand)'u';
                if (this.IsValid = (char.TryParse(data[0], out rawCommand) 
                    && Enum.TryParse(((int)rawCommand).ToString(), out command)))
                {
                    this.Command = command;
                    if (this.IsValid = InstanceResolverFor<IFSController>.Instance.Exists(data[1]))
                    {
                        this.PathToFile = data[1];
                    }
                }
            }
        }

        public static CommandParser Parse(string input)
        {
            return new CommandParser(input);
        }

        public enum ParserCommand
        {
            Identify = 'i',
            Trainning = 't'
        }

    }
}
