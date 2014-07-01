namespace Lunggo.Framework.SnowMaker
{
    public interface IUniqueIdGenerator
    {
        long NextId(string scopeName);
    }
}