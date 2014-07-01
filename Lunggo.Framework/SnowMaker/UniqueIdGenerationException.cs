using System;

namespace Lunggo.Framework.SnowMaker
{
    public class UniqueIdGenerationException : Exception
    {
        public UniqueIdGenerationException(string message)
            : base(message)
        {
        }
    }
}