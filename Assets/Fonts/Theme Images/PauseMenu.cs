using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance; // Singleton instance
    public GameObject pauseMenuUI;    // Assign the UI Panel in Inspector
    private bool isPaused = false;

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
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume game
        isPaused = false;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
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
