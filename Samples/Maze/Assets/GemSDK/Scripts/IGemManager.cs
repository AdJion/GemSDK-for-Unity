using System;

namespace GemSDK.Unity
{
    public interface IGemManager
    {
        void Connect();
        void Disconnect();
        IGem GetGem(String address);
    }
}
