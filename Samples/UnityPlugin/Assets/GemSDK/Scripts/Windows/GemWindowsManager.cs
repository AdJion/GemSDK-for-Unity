﻿using System.Threading;
using System;
using System.Collections.Generic;
using GemSDK.Unity;

namespace GemSDK.Unity
{
    public class GemWindowsService : IGemManager
    {
        Dictionary<String, WindowsGem> gems;

        public GemWindowsService()
        {
            gems = new Dictionary<String, WindowsGem>();
        }

        public void Connect()
        {
            NativeWrapper.gemInitialize();
        }

        public void Disconnect()
        {
            foreach (WindowsGem gem in gems.Values)
            {
                gem.Release();
            }

            gems.Clear();
            
            NativeWrapper.gemTerminate();
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