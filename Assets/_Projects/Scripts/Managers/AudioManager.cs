using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Local Audio Clips")]
    [SerializeField] private List<AudioClip> localAudioClips;  // Load these in Unity Editor
    
    [Header("Cloud Audio URLs")]
    [SerializeField] private List<string> cloudAudioUrls;      // URLs for cloud audio files
    
    private Dictionary<string, AudioClip> audioCache = new Dictionary<string, AudioClip>();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeLocalAudioCache();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeLocalAudioCache()
    {
        foreach (var clip in localAudioClips)
        {
            if (clip != null)
                audioCache[clip.name] = clip;
        }
    }

    public async Task PlayAudio(string clipName, AudioSource audioSource)
    {
        if (!audioCache.TryGetValue(clipName, out AudioClip clip))
        {
            clip = await DownloadAudioFromCloud(clipName);
            if (clip == null)
            {
                Debug.LogError($"Audio clip '{clipName}' not found in cache or cloud.");
                return;
            }
            audioCache[clipName] = clip;
        }

        audioSource.clip = clip;
        audioSource.Play();
    }

    private async Task<AudioClip> DownloadAudioFromCloud(string clipName)
    {
        var url = cloudAudioUrls.Find(url => url.Contains(clipName));
        if (string.IsNullOrEmpty(url))
        {
            Debug.LogError($"No URL found for clip '{clipName}'.");
            return null;
        }

        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error downloading audio clip '{clipName}': {request.error}");
                return null;
            }

            return DownloadHandlerAudioClip.GetContent(request);
        }
    }

    public bool RemoveAudioClipFromCache(string clipName)
    {
        if (audioCache.ContainsKey(clipName))
        {
            audioCache.Remove(clipName);
            return true;
        }
        return false;
    }
}
