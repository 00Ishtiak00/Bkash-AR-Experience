using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;
using System.Collections;

namespace MarksAssets.ScreenshotWebGL {
    #pragma warning disable 0162
    public class ScreenshotWebGL {
        private static event Action<byte[], int, int> screenshotEvent;
        private static event Action screenshotOptimizedEvent;
        private static event Action onLoadedEvent;

        [DllImport("__Internal", EntryPoint="ScreenshotOptimized_ScreenshotWebGL")]
        private static extern void ScreenshotOptimized_ScreenshotWebGL(Action screenshotCallback, string element = "body", string options = null, string type = "image/png", double encoderOptions = 0.92);

        [DllImport("__Internal", EntryPoint="Screenshot_ScreenshotWebGL")]
        private static extern void Screenshot_ScreenshotWebGL(Action<byte[], int, int, int> screenshotCallback, string element = "body", string options = null, string type = "image/png", double encoderOptions = 0.92);
        [DllImport("__Internal", EntryPoint="onLoaded_ScreenshotWebGL")]
        private static extern void onLoaded_ScreenshotWebGL(Action onLoadedCallback);
        [DllImport("__Internal", EntryPoint="didLoad_ScreenshotWebGL")]
        private static extern bool didLoad_ScreenshotWebGL();
        [DllImport("__Internal", EntryPoint="getGamePositionX_ScreenshotWebGL")]
        private static extern double getGamePositionX_ScreenshotWebGL();
        [DllImport("__Internal", EntryPoint="getGamePositionY_ScreenshotWebGL")]
        private static extern double getGamePositionY_ScreenshotWebGL();
        [DllImport("__Internal", EntryPoint="getGameWidth_ScreenshotWebGL")]
        private static extern double getGameWidth_ScreenshotWebGL();
        [DllImport("__Internal", EntryPoint="getGameHeight_ScreenshotWebGL")]
        private static extern double getGameHeight_ScreenshotWebGL();
        [DllImport("__Internal", EntryPoint="getDOMProperty_ScreenshotWebGL")]
        private static extern double getDOMProperty_ScreenshotWebGL(string propertyPath);
        [DllImport("__Internal", EntryPoint="clearScreenshotBlob_ScreenshotWebGL")]
        private static extern void clearScreenshotBlob_ScreenshotWebGL();

        [MonoPInvokeCallback(typeof(Action<byte[], int, int, int>))]
        private static void screenshotCallback([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 1)] byte[] byteArray, int length,  int width, int height) {
            screenshotEvent?.Invoke(byteArray, width, height);
            screenshotEvent = null;
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void screenshotOptmizedCallback() {
            screenshotOptimizedEvent?.Invoke();
            screenshotOptimizedEvent = null;
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void onLoadedCallback() {
            onLoadedEvent?.Invoke();
            onLoadedEvent = null;
        }

        public static void screenshot(Action callback, string element = "body", string options = null, string type = "image/png", double encoderOptions = 0.92) {
            #if UNITY_WEBGL && !UNITY_EDITOR
                screenshotOptimizedEvent += callback;
                ScreenshotOptimized_ScreenshotWebGL(screenshotOptmizedCallback, element, options, type, encoderOptions);
            #endif
        }

        public static void screenshot(Action<byte[], int, int> callback, string element = "body", string options = null, string type = "image/png", double encoderOptions = 0.92) {
            #if UNITY_WEBGL && !UNITY_EDITOR
                screenshotEvent += callback;
                Screenshot_ScreenshotWebGL(screenshotCallback, element, options, type, encoderOptions);
            #endif
        }

        public static void onLoaded(Action callback) {
            #if UNITY_WEBGL && !UNITY_EDITOR
                onLoadedEvent += callback;
                onLoaded_ScreenshotWebGL(onLoadedCallback);
            #endif
        }

        public static bool didLoad() {
            #if UNITY_WEBGL && !UNITY_EDITOR
                return didLoad_ScreenshotWebGL();
            #endif
            return false;
        }

        public static double getDOMProperty(string propertyPath) {
            #if UNITY_WEBGL && !UNITY_EDITOR
                return getDOMProperty_ScreenshotWebGL(propertyPath);
            #endif
            return -1.0;
        }

        public static double getGamePositionX() {
            #if UNITY_WEBGL && !UNITY_EDITOR
                return getGamePositionX_ScreenshotWebGL();
            #endif
            return -1.0;
        }

        public static double getGamePositionY() {
            #if UNITY_WEBGL && !UNITY_EDITOR
                return getGamePositionY_ScreenshotWebGL();
            #endif
            return -1.0;
        }

        public static double getGameWidth() {
            #if UNITY_WEBGL && !UNITY_EDITOR
                return getGameWidth_ScreenshotWebGL();
            #endif
            return -1.0;
        }

        public static double getGameHeight() {
            #if UNITY_WEBGL && !UNITY_EDITOR
                return getGameHeight_ScreenshotWebGL();
            #endif
            return -1.0;
        }

        public static void clearScreenshotBlob() {
            #if UNITY_WEBGL && !UNITY_EDITOR
                clearScreenshotBlob_ScreenshotWebGL();
            #endif
        }
    }
}
