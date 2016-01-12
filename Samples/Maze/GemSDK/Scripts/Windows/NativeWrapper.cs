using System;
using GemSDK.Unity;
using System.Runtime.InteropServices;

public enum GemStatusCode : int
{
    Success = 0x00,
    FailedWin32 = 0x01,
    FailedBluetooth = 0x02,
    NotFound = 0x03,
    AlreadyConnected = 0x04,
    NotConnected = 0x05,
    InsufficientBuffer = 0x06,
    Empty = 0x07,
    InvalidParameter = 0x08,
    BadConfig = 0x0A,
    AlreadyInitialized = 0x0B,
    NotInitialized = 0x0C,
    Unknown = 0xFF
}

[StructLayout(LayoutKind.Sequential, Size = 42), Serializable]
internal struct GemDescription
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public byte[] address;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string deviceName;

    public GemRole role;
}

[StructLayout(LayoutKind.Sequential, Size = 97), Serializable]
internal struct GemInfo
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string name;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string firmwareVer;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string hardwareVer;

    public byte batteryLevel;
}

internal static class NativeWrapper
{
    private const string DLLName = "GemSDK";

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void gemOnCombinedDataReceived(IntPtr quaternion, IntPtr acceleration);

    [DllImport(DLLName, CallingConvention = CallingConvention.Cdecl)]
    public static extern GemStatusCode gemInitialize();

    [DllImport(DLLName, CallingConvention = CallingConvention.Cdecl)]
    public static extern GemStatusCode gemTerminate();

    [DllImport(DLLName, CallingConvention = CallingConvention.Cdecl)]
    public static extern GemStatusCode gemSetOnCombinedDataReceived(byte[] address, gemOnCombinedDataReceived func);

    [DllImport(DLLName, CallingConvention = CallingConvention.Cdecl)]
    public static extern GemStatusCode gemSetOnCombinedDataReceivedRole(GemRole role, gemOnCombinedDataReceived func);

    [DllImport(DLLName, CallingConvention = CallingConvention.Cdecl)]
    public static extern GemStatusCode gemConnect(byte[] address);

    [DllImport(DLLName, CallingConvention = CallingConvention.Cdecl)]
    public static extern GemStatusCode gemDisconnect(byte[] address);

    [DllImport(DLLName, CallingConvention = CallingConvention.Cdecl)]
    public static extern GemStatusCode gemConnectRole(GemRole role);

    [DllImport(DLLName, CallingConvention = CallingConvention.Cdecl)]
    public static extern GemStatusCode gemDisconnectRole(GemRole role);

    [DllImport(DLLName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong gemGetWin32LastError();

    [DllImport(DLLName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint gemGetDescriptionListCount();

    [DllImport(DLLName, CallingConvention = CallingConvention.Cdecl)]
    public static extern GemStatusCode gemGetDescriptionList([Out] GemDescription[] list, uint size);

    [DllImport(DLLName, CallingConvention = CallingConvention.Cdecl)]
    public static extern GemStatusCode gemGetInfo(byte[] address, out GemInfo gemInfo);
}