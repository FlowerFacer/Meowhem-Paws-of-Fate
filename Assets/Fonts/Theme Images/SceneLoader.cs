using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public AudioClip buttonClickSound; // ?? Assign sound in the Inspector
    [Range(0f, 1f)] public float volume = 0.5f; // ?? Adjustable volume

    public string sceneName; // Scene name

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // ?? Add AudioSource component
        audioSource.playOnAwake = false;
    }

    public void LoadScene()
    {
        PlayButtonClickSound();
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        PlayButtonClickSound();
        Debug.Log("Game is exiting...");
        Application.Quit(); // Closes the game
    }

    private void PlayButtonClickSound()
    {
        if (buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound, volume); // ?? Play click sound
        }
    }
}
