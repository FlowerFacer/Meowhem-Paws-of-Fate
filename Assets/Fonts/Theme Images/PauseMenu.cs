using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance; // Singleton instance
    public GameObject pauseMenuUI;    // Assign the UI Panel in Inspector
    public bool isPaused = false;

    // ⛔️ Scenes where the pause menu is disabled
    private string[] disabledScenes = { "MainMenu", "LoadingScreen", "Credits" };

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }
    }

    void Update()
    {
        // ❌ Don't allow pausing in specific scenes
        if (IsSceneRestricted()) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        if (IsSceneRestricted()) return; // ⛔️ Prevent pausing in disabled scenes

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Freeze game
        isPaused = true;

        // ✅ Make sure ALL movement stops
        AudioListener.pause = true;  // Pause all audio
        Cursor.visible = true;       // Show cursor if hidden
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // ✅ Resume game
        isPaused = false;

        AudioListener.pause = false;  // Resume audio
        Cursor.visible = false;       // Hide cursor if needed
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // ✅ Reset time scale when quitting

        isPaused = false;

        AudioListener.pause = false;  // Resume audio

        SceneManager.LoadScene("MenuScene");
    }

    // 🔍 **Check if the current scene is restricted**
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
