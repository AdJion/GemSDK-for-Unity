using System;
using UnityEngine;

namespace GemSDK.Unity
{
    public interface IGem
    {
        GemState State { get; }
        Quaternion Rotation { get; }
        Vector3 Acceleration { get; }
        PedometerData Pedometer { get; }
        GemSystemInfo SystemInfo { get; }
        void Release();

        void Calibrate();
        void Reconnect();
        void setPedometerActive(bool isActive);
        bool CheckTapOccured();
    }
}
