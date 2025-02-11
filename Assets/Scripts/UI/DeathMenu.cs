using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public static DeathMenu instance; // Singleton instance
    public GameObject deathMenuUI;    // Assign the UI Panel in Inspector
    private bool isDieMenu = false;

    private string[] disabledScenes = { "MenuScene", "LoadingScene", "LoadingScene2", "LoadingScene3", "Credits" };

    void Awake()
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
    }

    public void ShowDeathMenu()
    {
        if (IsSceneRestricted()) return; // Prevent in restricted scenes

        deathMenuUI.SetActive(true);
        Time.timeScale = 0f; // ✅ Freeze game
        isDieMenu = true;

        AudioListener.pause = true; // ✅ Pause all game audio
        Cursor.visible = true; // ✅ Show cursor for UI navigation
    }

    public void RetryGame()
    {
        if (!isDieMenu) return; // ✅ Prevent accidental calls

        isDieMenu = false;
        Time.timeScale = 1f; // ✅ Resume game
        AudioListener.pause = false;
        Cursor.visible = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ✅ Reload current level
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        isDieMenu = false;
        AudioListener.pause = false;
        SceneManager.LoadScene("MenuScene"); // ✅ Ensure MainMenu exists in build settings
    }

    private bool IsSceneRestricted()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        foreach (string scene in disabledScenes)
        {
            if (currentScene == scene) return true;
        }
        return false;
    }
}
