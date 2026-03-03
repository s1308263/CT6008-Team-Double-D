using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    [SerializeField] private GameObject mainMenuPrefab, settingsPrefab;

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

    public void ShowSettings() {
        mainMenuPrefab.SetActive(false);
        settingsPrefab.SetActive(true);
    }

    public void HideSettings() {
        mainMenuPrefab.SetActive(true);
        settingsPrefab.SetActive(false);
    }



}
