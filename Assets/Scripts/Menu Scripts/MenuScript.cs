using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MenuScript : MonoBehaviour {

    [SerializeField] private GameObject mainMenuPrefab, settingsPrefab;

    bool isSettings = false, isPaused;

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

    public void OnPause(InputAction.CallbackContext context) {
        if (!isPaused) {
            transform.GetChild(3).transform.gameObject.SetActive(true);
            Time.timeScale = 0;
            isPaused = true;
        }
        else {
            transform.GetChild(3).transform.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
