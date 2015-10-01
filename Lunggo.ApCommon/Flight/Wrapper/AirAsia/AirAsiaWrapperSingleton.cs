namespace Lunggo.ApCommon.Flight.Wrapper.AirAsia
{
    internal partial class AirAsiaWrapper : WrapperBase
    {
        private static readonly AirAsiaWrapper Instance = new AirAsiaWrapper();
        private bool _isInitialized;
        private static readonly AirAsiaClientHandler Client = AirAsiaClientHandler.GetClientInstance();

        private AirAsiaWrapper()
        {
            
        }

        internal static AirAsiaWrapper GetInstance()
        {
            return Instance;
        }

        internal void Init()
        {
            if (!_isInitialized)
            {
                Client.Init();
                _isInitialized = true;
            }
        }
    }
}
