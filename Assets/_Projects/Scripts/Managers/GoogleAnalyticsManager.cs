using System.Runtime.InteropServices;
using UnityEngine;

public class GoogleAnalyticsManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void SendGTagEvent(string eventName, string category, string label, int value);

    public static void TrackEvent(string eventName, string category, string label, int value)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        SendGTagEvent(eventName, category, label, value);
#else
        Debug.Log($"Event: {eventName}, Category: {category}, Label: {label}, Value: {value}");
#endif
    }
}