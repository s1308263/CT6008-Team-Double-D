using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class MenuScript : MonoBehaviour {

    [SerializeField] private GameObject mainMenuPrefab, settingsPrefab;
    [SerializeField] private Slider graphicsSlider;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioMixer mixer;

    bool isSettings = false, isPaused = false;

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

    public void SettingsMenu() {
        if (isSettings == false) {
            settingsPrefab.SetActive(true);
            RefreshSettings();
            isSettings = true;
        }
        else if (isSettings == true){
            settingsPrefab.SetActive(false); 
            isSettings = false;
        }
    }


    public void RefreshSettings() {
        graphicsSlider.value = Settings.graphicsQuality;
        volumeSlider.value = Settings.volume;

        ApplySettings();
    }

    public void ApplySettings() {
        Settings.graphicsQuality = (int)graphicsSlider.value;
        Settings.volume = volumeSlider.value;

        QualitySettings.SetQualityLevel(Settings.graphicsQuality);
        mixer.SetFloat("Master", Mathf.Log10(Settings.volume) * 20);
    }

    public void BackToMainMenu() {
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }

    public void OnPause(InputAction.CallbackContext context) {
        Debug.Log("PAUSE PRESSED");
        if (context.performed) {
            switch (isPaused) {
                case false:
                transform.GetChild(4).transform.gameObject.SetActive(true);
                Cursor.visible = true;
                Time.timeScale = 0;
                isPaused = true;
                break;
                case true:
                transform.GetChild(4).transform.gameObject.SetActive(false);
                Cursor.visible = false;
                Time.timeScale = 1;
                isPaused = false; 
                break;
            }
        }
    }
}
