using CommandLine;
using partycli.Domain.Enums;

namespace partycli.Options
{
    public class ArgumentOptions
    {

        [Value(0, MetaName = "command", Required = true, HelpText = "The primary command.")]
        public ParentArgument PrimaryArgument { get; set; }

        [Option("local", Required = false, HelpText = "Get local server list.")]
        public bool IsLocal { get; set; }

        [Option("TCP", Required = false, HelpText = "Get servers by TCP protocol.")]
        public bool IsTcp { get; set; }

        [Option("france", Required = false, HelpText = "Get servers in France.")]
        public bool IsFrance { get; set; }
    }
}
