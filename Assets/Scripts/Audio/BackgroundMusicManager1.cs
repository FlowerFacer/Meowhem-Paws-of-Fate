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
        [Range(0f, 1f)] public float volume = 0.5f; // Volume for this scene
    }

    public AudioClip defaultMusic; // Default music if no specific scene music is set
    [Range(0f, 1f)] public float defaultVolume = 0.5f; // Default music volume
    public SceneMusic[] sceneMusicList; // List of scene-specific music

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    private void PlayMusicForScene(string sceneName)
    {
        AudioClip sceneMusic = defaultMusic;
        float sceneVolume = defaultVolume;

        // Find music & volume for the current scene
        foreach (var entry in sceneMusicList)
        {
            if (entry.sceneName == sceneName)
            {
                sceneMusic = entry.musicClip;
                sceneVolume = entry.volume;
                break;
            }
        }

        // If the same music is already playing, do nothing
        if (audioSource.clip == sceneMusic) return;

        // Play the new music with its scene-specific volume
        audioSource.Stop();
        audioSource.clip = sceneMusic;
        audioSource.volume = sceneVolume;
        audioSource.Play();
    }

    public void SetGlobalVolume(float volume)
    {
        float clampedVolume = Mathf.Clamp01(volume);
        audioSource.volume = clampedVolume;
    }
}
