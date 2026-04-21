using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    [SerializeField] private GameObject mainMenuPrefab, settingsPrefab;

    bool isSettings = false;

    private void Awake() {
        Time.timeScale = 1;
    }

    public void PlayTutorial() {
        SceneManager.LoadScene("Tutorial Level");
    }

    public void PlayNoTut() {
        SceneManager.LoadScene("Level 1");
    }

    public void Quit() {
        Debug.Log("Game Quit");
        Application.Quit();
    }

    public void Settings() {
        if (isSettings == false) {
            settingsPrefab.SetActive(true);
            isSettings = true;
        }
        else if (isSettings == true){
            settingsPrefab.SetActive(false); 
            isSettings = false;
        }
    }

    public void BackToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}
