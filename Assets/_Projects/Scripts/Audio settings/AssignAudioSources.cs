using UnityEngine;
using UnityEngine.UI;

public class AssignAudioSources : MonoBehaviour
{
    public GameObject buttonParent;       // Parent GameObject containing all buttons
    public GameObject audioSourceParent; // Parent GameObject containing all AudioSources

    void Start()
    {
        Button[] buttons = buttonParent.GetComponentsInChildren<Button>();
        AudioSource[] audioSources = audioSourceParent.GetComponentsInChildren<AudioSource>();

        if (buttons.Length != audioSources.Length)
        {
            Debug.LogError("The number of buttons and audio sources does not match!");
            return;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            AudioPitchHandler handler = buttons[i].gameObject.AddComponent<AudioPitchHandler>();
            handler.linkedAudioSource = audioSources[i];

            // Automatically initialize audio sources
            audioSources[i].pitch = 0;
            audioSources[i].Play();

            // Assign button click event
            buttons[i].onClick.AddListener(handler.SetPitchToOne);
        }

        Debug.Log("Audio sources and buttons linked successfully!");
    }
}