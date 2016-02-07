using System;
using UnityEngine;

namespace GemSDK.Unity
{
    public interface IGem
    {
        /// <summary>
        /// Connection state
        /// </summary>
        GemState State { get; }

        /// <summary>
        /// Rotation
        /// </summary>
        Quaternion Rotation { get; }

        /// <summary>
        /// Acceleration
        /// </summary>
        Vector3 Acceleration { get; }
        
        /// <summary>
        /// Pedometer
        /// </summary>
        /// <remarks>Not available on Windows</remarks> 
        PedometerData Pedometer { get; }

        /// <summary>
        /// Detailed information about the Gem: model, address, firmware version, hardware version 
        ///</summary>
        ///<remarks>Not available on Windows</remarks> 
        GemSystemInfo SystemInfo { get; }

        /// <summary>
        /// Disconnect the Gem
        /// </summary>
        void Release();

        /// <summary>
        /// Use current rotation as origin. All next rotaions will be relative to it
        /// </summary>
        void Calibrate();

        /// <summary>
        /// Attempt to connect again to the Gem
        /// </summary>
        /// <remarks>Not available on Windows</remarks> 
        void Reconnect();

        /// <summary>
        /// Enable/Disable pedometer data streaming
        /// </summary>
        /// <param name="isActive"><code>true</code> to enbale or <code>false</code> to disable</param>
        /// <remarks>Not available on Windows</remarks> 
        void SetPedometerActive(bool isActive);

        /*/// <summary>
        /// Set current pedometer values to 0. Be careful: with firmware version below 1.2.0 will cause restart of the Gem 
        /// </summary>
        /// <remarks>Not available on Windows</remarks> 
        void ResetPedometer();*/

        /// <summary>
        /// Check if at least one tap was catched between calls
        /// </summary>
        /// <returns>True if tap has been catched</returns>
        /// <remarks>Not available on Windows</remarks> 
        bool CheckTapOccured();
    }
}
