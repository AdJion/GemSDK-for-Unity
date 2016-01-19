using System.Threading;
using System;
using System.Collections.Generic;
using GemSDK.Unity;
using UnityEngine;

namespace GemSDK.Unity
{
    public class GemWindowsManager : IGemManager
    {
        Dictionary<String, WindowsGem> gems;

        public GemWindowsManager()
        {
            gems = new Dictionary<String, WindowsGem>();
        }

        public void Connect()
        {
            var result = NativeWrapper.gemInitialize();
            Debug.Log("GemSDK: initialization -> " + result.ToString());
        }

        public void Disconnect()
        {
            foreach (WindowsGem gem in gems.Values)
            {
                gem.Release();
            }

            gems.Clear();

            var result = NativeWrapper.gemTerminate();
            Debug.Log("GemSDK: closing -> " + result.ToString());
        }

        public IGem GetGem(string address)
        {
            WindowsGem gem;
            bool isGemExists = gems.TryGetValue(address, out gem);

            if (!isGemExists)
            {
                gem = new WindowsGem(address);
                gems.Add(address, gem);
                gem.Connect();
            }

            return gem;
        }
    }
}