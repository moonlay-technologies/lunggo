namespace Lunggo.ApCommon.Flight.Wrapper.Mystifly
{
    internal partial class MystiflyWrapper : WrapperBase
    {
        private static readonly MystiflyWrapper Instance = new MystiflyWrapper();
        private bool _isInitialized;
        private static readonly MystiflyClientHandler Client = MystiflyClientHandler.GetClientInstance();

        private MystiflyWrapper()
        {
            
        }

        internal static MystiflyWrapper GetInstance()
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
