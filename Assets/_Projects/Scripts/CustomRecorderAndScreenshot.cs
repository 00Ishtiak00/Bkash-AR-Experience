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
        statusText.text = "Recording started...";
    }

    private void StopRecording()
    {
        RecorderWebGL.Stop(OnRecordingStopped);
        stopButton.gameObject.SetActive(false);
        recordButton.gameObject.SetActive(true);
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
        yield return new WaitForEndOfFrame();
        screenshotTexture = ScreenCapture.CaptureScreenshotAsTexture();
        screenshotFile = screenshotTexture.EncodeToPNG();
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
}
