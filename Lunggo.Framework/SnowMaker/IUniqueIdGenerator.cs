using System;

namespace Lunggo.Framework.SnowMaker
{
    public interface IUniqueIdGenerator
    {
        long NextId(string scopeName);
        void SetIdInitialValue(String name, long value);
        long GetIdInitialValue(String name);
    }
}