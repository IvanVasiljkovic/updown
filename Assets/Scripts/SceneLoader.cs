using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string[] sceneNames; // Array to store the names of the scenes you want to load

    public void LoadScene(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < sceneNames.Length)
        {
            Debug.Log("Loading scene: " + sceneNames[sceneIndex]);
            SceneManager.LoadScene(sceneNames[sceneIndex]); // Load the selected scene by its name
        }
        else
        {
            Debug.LogWarning("Invalid scene index!");
        }
    }


    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(sceneNames[0]); // Load the first scene to reset the game
    }
}
