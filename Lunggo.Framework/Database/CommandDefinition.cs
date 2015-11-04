using System.Data;

namespace Lunggo.Framework.Database
{
    public class CommandDefinition
    {
        public int CommandTimeout { get; set; }
        public bool Buffered { get; set; }
        public CommandType CommandType { get; set; }

        public static CommandDefinition GetDefaultDefinition()
        {
            return new CommandDefinition 
            {
                CommandTimeout = 10,
                Buffered = true,
                CommandType = CommandType.Text
            };
        }
    }
}
