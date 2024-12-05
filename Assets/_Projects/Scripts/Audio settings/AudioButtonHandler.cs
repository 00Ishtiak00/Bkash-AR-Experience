using UnityEngine;

public class AudioPitchHandler : MonoBehaviour
{
    public AudioSource linkedAudioSource; // Assign the corresponding AudioSource in the Inspector

    private void Start()
    {
        if (linkedAudioSource != null)
        {
            linkedAudioSource.pitch = 0; // Mute by setting pitch to 0
            linkedAudioSource.Play();    // Start playing the audio
        }
        else
        {
            Debug.LogWarning($"No AudioSource assigned to {gameObject.name}");
        }
    }

    public void SetPitchToOne()
    {
        if (linkedAudioSource != null)
        {
            linkedAudioSource.pitch = 1; // Enable audio by setting pitch to 1
        }
        else
        {
            Debug.LogWarning($"No AudioSource assigned to {gameObject.name}");
        }
    }
}