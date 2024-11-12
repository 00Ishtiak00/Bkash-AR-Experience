using System;
using UnityEngine;
using System.Runtime.InteropServices;

public class OpenSmartLink : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void OpenDeepLink(string url);

    public void OpenLink()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        OpenDeepLink("https://f8uqp.app.goo.gl/smartlink");
#else
        Debug.Log("OpenDeepLink is only supported in WebGL builds.");
#endif
    }
}