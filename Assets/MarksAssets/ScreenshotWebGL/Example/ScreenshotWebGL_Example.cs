using UnityEngine;
using MarksAssets.ScreenshotWebGL;
using System.Collections;
using System;

public class ScreenshotWebGL_Example : MonoBehaviour {
    //#pragma warning disable 8321
    private Texture2D texture2D;
    void Awake() {
        ScreenshotWebGL.onLoaded(onLoadedCallback);
    }

    private void screenshotCallback(byte[] bytes, int width, int height) {
        texture2D = new Texture2D(width, height);
        texture2D.LoadImage(bytes);
        GetComponent<Renderer>().material.mainTexture = texture2D;
        transform.localScale = new Vector3((float)width/(float)height, 1.0f , 1.0f) * 5.0f;
    }


    private void onLoadedCallback() {
        IEnumerator  InnerLoad() {
            yield return new WaitForSeconds(5);//wait 5 seconds to take screenshot
            yield return new WaitForEndOfFrame();
            ScreenshotWebGL.screenshot(screenshotCallback, "body", $"{{\"backgroundColor\": \"#FF0000\", \"scale\": {2.0 * ScreenshotWebGL.getDOMProperty("window.devicePixelRatio")} }}");//takes screenshot of whole screen. This will show everything on the screen, which may include the html's background(if the game is not fullscreen) and other html overlay elements(if any) on top of the Unity's game, besides the game itself obviously. The color of the background was changed to red, and the resolution of the image is doubled for higher quality.
            //ScreenshotWebGL.screenshot(screenshotCallback, "canvas");//take screenshot of game canvas only. Only the game will be seen, even if it doesn't cover the whole screen. This line of code assumes that there is only one canvas, or if there is more, the first one is the game's canvas.
        }

        StartCoroutine(InnerLoad());
    }
}
