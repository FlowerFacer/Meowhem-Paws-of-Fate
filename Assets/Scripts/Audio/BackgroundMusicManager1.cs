using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager instance; // Singleton instance

    [System.Serializable]
    public class SceneMusic
    {
        public string sceneName; // Scene name
        public AudioClip musicClip; // Music to play in this scene
    }

    public AudioClip defaultMusic; // Default music if no specific scene music is set
    public float musicVolume = 0.5f; // Music volume
    public SceneMusic[] sceneMusicList; // List of scene-specific music

    private AudioSource audioSource;

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
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = musicVolume;

        SceneManager.sceneLoaded += OnSceneLoaded; // Listen to scene load events
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    private void PlayMusicForScene(string sceneName)
    {
        AudioClip sceneMusic = null;

        // Find music for the current scene
        foreach (var entry in sceneMusicList)
        {
            if (entry.sceneName == sceneName)
            {
                sceneMusic = entry.musicClip;
                break;
            }
        }

        // Use default music if no scene-specific music is found
        if (sceneMusic == null)
        {
            sceneMusic = defaultMusic;
        }

        // If the same music is already playing, do nothing
        if (audioSource.clip == sceneMusic) return;

        // Play the new music
        audioSource.Stop();
        audioSource.clip = sceneMusic;
        audioSource.Play();
    }

    public void SetVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume); // Clamp between 0 and 1
        audioSource.volume = musicVolume;
    }
}
