using UnityEngine;
using UnityEngine.UI;
using MarksAssets.RecorderWebGL;
using MarksAssets.ShareNSaveWebGL;
using System.Collections;
using TMPro;
using status = MarksAssets.ShareNSaveWebGL.ShareNSaveWebGL.status;

public class CustomRecorderAndScreenshot : MonoBehaviour
{
    // UI References
    public Button recordButton;
    public Button stopButton;
    public Button screenshotButton;
    public Button shareButton;
    public TMP_Text statusText;

    // Private Variables
    private byte[] recordedBytes;
    private Texture2D screenshotTexture;
    private byte[] screenshotFile;
    private RecorderWebGL.MediaRecorderOptions mediaRecorderOptions = new RecorderWebGL.MediaRecorderOptions("video/webm;codecs=vp8,opus");

    private void Start()
    {
        // Assign button listeners
        recordButton.onClick.AddListener(StartRecording);
        stopButton.onClick.AddListener(StopRecording);
        screenshotButton.onClick.AddListener(CaptureScreenshot);
        shareButton.onClick.AddListener(ShareContent);

        // Initialize button states
        stopButton.gameObject.SetActive(false);
        shareButton.gameObject.SetActive(false);
    }

    #region Recording Methods
    public void StartRecording()
    {
        RecorderWebGL.CreateMediaRecorder(OnCreateRecorder, mediaRecorderOptions, false, true); // In-game audio
        recordButton.gameObject.SetActive(false);
        stopButton.gameObject.SetActive(true);
        //SetButtonTransparency(stopButton, 0.5f); // Set stop button transparency to 50%
        DisableButtonsDuringAction(true);       // Disable Share and Screenshot buttons
        statusText.text = "Recording started...";
    }

    private void StopRecording()
    {
        RecorderWebGL.Stop(OnRecordingStopped);
        stopButton.gameObject.SetActive(false);
        SetButtonTransparency(stopButton, 1f); // Reset stop button transparency
        recordButton.gameObject.SetActive(true);
        EnableButtonsAfterAction();
        statusText.text = "Recording stopped.";
    }

    private void OnCreateRecorder(RecorderWebGL.status recorderStatus)
    {
        if (recorderStatus == RecorderWebGL.status.Success)
        {
            RecorderWebGL.Start();
            statusText.text = "Recording initialized.";
        }
        else
        {
            statusText.text = $"Recording initialization failed: {recorderStatus}";
        }
    }

    private void OnRecordingStopped(byte[] bytes, int size)
    {
        recordedBytes = bytes;
        statusText.text = $"Recording saved. Size: {size} bytes.";
        shareButton.gameObject.SetActive(true); // Allow sharing after recording
    }
    #endregion

    #region Screenshot Methods
    public void CaptureScreenshot()
    {
        StartCoroutine(CaptureScreenshotCoroutine());
    }

    private IEnumerator CaptureScreenshotCoroutine()
    {
        DisableButtonsDuringAction(true);      // Disable Stop and Share buttons
        SetButtonTransparency(screenshotButton, 0f); // Set screenshot button transparency to 0
        yield return new WaitForSeconds(1f);  // Wait 1 second

        screenshotTexture = ScreenCapture.CaptureScreenshotAsTexture();
        screenshotFile = screenshotTexture.EncodeToPNG();
        SetButtonTransparency(screenshotButton, 1f); // Reset screenshot button transparency
        EnableButtonsAfterAction();
        statusText.text = "Screenshot captured.";
        shareButton.gameObject.SetActive(true); // Allow sharing after screenshot
    }
    #endregion

    #region Sharing Methods
    private void ShareContent()
    {
        if (recordedBytes != null)
        {
            ShareVideo();
        }
        else if (screenshotFile != null)
        {
            ShareScreenshot();
        }
        else
        {
            statusText.text = "Nothing to share!";
        }
    }

    private void ShareVideo()
    {
        var shareStatus = ShareNSaveWebGL.CanShare(recordedBytes, "video/webm");
        if (shareStatus == status.Success)
        {
            ShareNSaveWebGL.Share(OnShareCallback, recordedBytes, "video/webm", "recorded_video.webm");
        }
        else
        {
            statusText.text = $"Unable to share video: {shareStatus}";
        }
    }

    private void ShareScreenshot()
    {
        var shareStatus = ShareNSaveWebGL.CanShare(screenshotFile, "image/png");
        if (shareStatus == status.Success)
        {
            ShareNSaveWebGL.Share(OnShareCallback, screenshotFile, "image/png", "screenshot.png");
        }
        else
        {
            statusText.text = $"Unable to share screenshot: {shareStatus}";
        }
    }

    private void OnShareCallback(status shareStatus)
    {
        if (shareStatus == status.Success)
        {
            statusText.text = "Content shared successfully!";
        }
        else
        {
            statusText.text = $"Share failed: {shareStatus}";
        }
    }
    #endregion

    #region Helper Methods
    private void DisableButtonsDuringAction(bool includeScreenshot)
    {
        shareButton.interactable = false;
        stopButton.interactable = false;
        if (includeScreenshot)
        {
            screenshotButton.interactable = false;
        }
    }

    private void EnableButtonsAfterAction()
    {
        shareButton.interactable = true;
        stopButton.interactable = true;
        screenshotButton.interactable = true;
    }

    private void SetButtonTransparency(Button button, float alpha)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            Color color = buttonImage.color;
            color.a = alpha;
            buttonImage.color = color;
        }
    }
    #endregion
}
