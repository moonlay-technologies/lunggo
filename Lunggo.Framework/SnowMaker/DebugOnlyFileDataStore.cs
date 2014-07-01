using System.IO;

namespace Lunggo.Framework.SnowMaker
{
    public class DebugOnlyFileDataStore : IOptimisticDataStore
    {
        const string SeedValue = "1";

        readonly string _directoryPath;

        public DebugOnlyFileDataStore(string directoryPath)
        {
            this._directoryPath = directoryPath;
        }

        public string GetData(string blockName)
        {
            var blockPath = Path.Combine(_directoryPath, string.Format("{0}.txt", blockName));
            try
            {
                return File.ReadAllText(blockPath);
            }
            catch (FileNotFoundException)
            {
                using (var file = File.Create(blockPath))
                using (var streamWriter = new StreamWriter(file))
                {
                    streamWriter.Write(SeedValue);
                }
                return SeedValue;
            }
        }

        public bool TryOptimisticWrite(string blockName, string data)
        {
            var blockPath = Path.Combine(_directoryPath, string.Format("{0}.txt", blockName));
            File.WriteAllText(blockPath, data);
            return true;
        }
    }
}
