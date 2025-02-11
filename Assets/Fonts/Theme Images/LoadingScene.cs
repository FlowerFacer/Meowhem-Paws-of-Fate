using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public string sceneName; // Scene name

    // Function to load a scene by name
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
