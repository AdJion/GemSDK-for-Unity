using UnityEngine;
using System;
using System.Collections;

namespace GemSDK.Unity
{
    /// <summary>
    /// Represents connection to remote Gem Service regardless of platform
    /// </summary>
    public class GemManager
    {
        private static IGemManager instance; 
     
        /// <summary>
        /// Singleton instance
        /// </summary>
        public static IGemManager Instance
        {
            get
            {
                if (instance == null)
                {
                    #if (UNITY_ANDROID)
                        instance = new GemAndroidManager();
                    #elif (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
                        instance = new GemWindowsService();
                    #else
                        Debug.Log("GemSDK doesn't support this platform");
                    #endif
                }     
                
                return instance;
            }
        }
    }
}
