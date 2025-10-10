using UnityEngine;

public enum DebugType
{
    Log,
    Warrning,
    Error
}

public static class Debugger
{
    public static bool EnableDebugs = true;

    public static void Log(object message)
    {
        if (EnableDebugs) Debug.Log(message);
    }

    public static void LogWarning(object message)
    {
        if (EnableDebugs) Debug.LogWarning(message);
    }

    public static void LogError(object message)
    {
        if (EnableDebugs) Debug.LogError(message);
    }
}
