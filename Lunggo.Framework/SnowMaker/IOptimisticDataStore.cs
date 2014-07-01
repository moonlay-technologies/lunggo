namespace Lunggo.Framework.SnowMaker
{
    public interface IOptimisticDataStore
    {
        string GetData(string blockName);
        bool TryOptimisticWrite(string blockName, string data);
    }
}
