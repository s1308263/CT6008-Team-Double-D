using Unity.VisualScripting;
using UnityEngine;

public class Level_Complete : MonoBehaviour {

    [SerializeField] private GameObject winMenu;

    public void NextLevel() {
        Time.timeScale = 0.01f;
        Cursor.visible = true;
        winMenu.SetActive(true);
    }
}
