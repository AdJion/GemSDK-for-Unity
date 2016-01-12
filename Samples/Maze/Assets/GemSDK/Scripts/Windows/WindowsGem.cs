using System;
using UnityEngine;
using System.Runtime.InteropServices;

namespace GemSDK.Unity
{
    public class WindowsGem : IGem
    {
        private Quaternion _last;
        private readonly object lastMutex = new object();
        private Vector3 _lastAcceleration;
        private readonly object lastAccelerationMutex = new object();
        private Quaternion reference;
        private NativeWrapper.gemOnCombinedDataReceived onCombinedDataRecieved;
        private byte[] address;
        private string addressStr;

        private Quaternion Last
        {
            get
            {
                Quaternion result;

                lock(lastMutex)
                {
                    result = _last;
                }

                return result;
            }

            set
            {
                lock(lastMutex)
                {
                    _last = value;
                }
            }
        }
        
        private Vector3 LastAcceleration
        {
            get
            {
                Vector3 result;

                lock (lastAccelerationMutex)
                {
                    result = _lastAcceleration;
                }

                return result;
            }

            set
            {
                lock (lastAccelerationMutex)
                {
                    _lastAcceleration = value;
                }
            }
        }
        
        public WindowsGem(string address)
        {
            _last = Quaternion.identity;
            reference = Quaternion.identity;
            _lastAcceleration = Vector3.zero;
            State = GemState.Disconnected;
            onCombinedDataRecieved = new NativeWrapper.gemOnCombinedDataReceived(CombinedDataReceived);

            string[] str = address.Split(':');
            var addrBytes = new byte[str.Length];

            for (int i = 0; i < addrBytes.Length; i++)
            {
                addrBytes[i] = Byte.Parse(str[i], System.Globalization.NumberStyles.HexNumber);

            }

            this.address = addrBytes;
            this.addressStr = address;

            NativeWrapper.gemSetOnCombinedDataReceived(addrBytes, onCombinedDataRecieved);
        }

        public GemState State { get; private set; }
        
        public Quaternion Rotation { get { return reference * Last; } }

        public Vector3 Acceleration { get { return LastAcceleration; } }

        public void Calibrate()
        {
            reference = Quaternion.Inverse(Last);   
        }

        public void Connect()
        {
            Debug.Log("gemConnectRole(" + addressStr + ") > " + NativeWrapper.gemConnect(address).ToString());
        }

        public void Release()
        {
            Debug.Log("gemDisconnectRole(" + addressStr + ") > " + NativeWrapper.gemDisconnect(address).ToString());
        }

        #region not implemented yet
        public void Reconnect()
        {
            //throw new NotImplementedException();
        }

        public PedometerData Pedometer { get; private set; }
        public GemSystemInfo SystemInfo { get; private set; }

        public void setPedometerActive(bool isActive)
        {
            //throw new NotImplementedException();        
        }

        public bool CheckTapOccured()
        {
            return false;
        }

        #endregion

        private void CombinedDataReceived(IntPtr quaternion, IntPtr acceleration)
        {
            float[] q = new float[4];
            Marshal.Copy(quaternion, q, 0, 4);

            float[] acc = new float[3];
            Marshal.Copy(acceleration, acc, 0, 3);

            Last = new Quaternion(q[0], q[1], q[2], q[3]);
            LastAcceleration = new Vector3(acc[0], acc[1], acc[2]);
        }
    }
}