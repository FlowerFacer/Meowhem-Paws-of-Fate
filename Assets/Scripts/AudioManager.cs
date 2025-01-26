using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private AudioSource audioSource;

    [Range(0f, 1f)] public float masterVolume = 1f; // Global volume multiplier

    [System.Serializable]
    public class SoundSettings
    {
        public string name; // Name for clarity (e.g., "Bird Chirp 1")
        public AudioClip clip; // The sound clip
        [Range(0f, 1f)] public float volume = 1f; // Volume for this sound
    }

    public List<SoundSettings> sounds = new List<SoundSettings>(); // List of sound settings

    private Dictionary<AudioClip, float> soundVolumes; // Dictionary for quick access

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Initialize the soundVolumes dictionary
        soundVolumes = new Dictionary<AudioClip, float>();
        foreach (var sound in sounds)
        {
            if (sound.clip != null)
            {
                soundVolumes[sound.clip] = sound.volume;
                Debug.Log($"Sound {sound.name} added with volume {sound.volume}");
            }
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null && soundVolumes.ContainsKey(clip))
        {
            float volume = soundVolumes[clip] * masterVolume; // Apply individual and global volume
            Debug.Log($"Playing sound {clip.name} at volume {volume}");
            audioSource.PlayOneShot(clip, volume);
        }
        else
        {
            Debug.LogWarning($"AudioClip {clip.name} not found in AudioManager or volume not set.");
        }
    }
}
