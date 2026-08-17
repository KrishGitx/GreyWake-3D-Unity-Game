using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{

    // public GameObject restartMenu;
    public GameObject player;
    
    public void restartGame()
    {
        Time.timeScale =1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Cursor.lockState = CursorLockMode.Locked;
        // gameObject.SetActive(false);
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void playGame()
    {
        SceneManager.LoadScene(1);
    }
    public void muteAudio()
    {
        AudioListener.pause = true;

    }
    public void SetQuality(int level)
    {
        QualitySettings.SetQualityLevel(level);
        PlayerPrefs.SetInt("QualityLevel", level);
        PlayerPrefs.Save();
    }
}
