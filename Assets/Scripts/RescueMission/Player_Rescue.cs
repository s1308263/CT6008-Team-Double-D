using UnityEngine;

public class Player_Rescue : MonoBehaviour {

    public int[] amountInShip;
    public int totalNeedRescuing;
    [SerializeField] private int shipCapacity;

    [SerializeField] private GameObject everyoneRescuedText;

    public GameObject rescuePickupInScene;

    float rescuedTimer;
    bool canShowRescueText;

    private void Awake() {
        totalNeedRescuing = 1;   //Random.Range(1, 5);
        //RANDOM SPAWN PLATFORMS HERE

        shipCapacity = 1;
        amountInShip = new int[shipCapacity];
        canShowRescueText = true;
    }

    private void Update() {
        if (totalNeedRescuing <= 0 && canShowRescueText == true) {
            //Show all rescued text here
            totalNeedRescuing = 0;
            everyoneRescuedText.SetActive(true);
            rescuedTimer += 1 * Time.deltaTime;
            if(rescuedTimer >= 10) {
                everyoneRescuedText.SetActive(false);
                canShowRescueText = false;
            }
            Debug.Log("Everyone Rescued");
        }
    }


    public void AddCapacity() {
        shipCapacity++;
    }

    public void Rescued() {
        totalNeedRescuing--;
    }

    public void AddPassenger() {
        for (int i = 0;i < amountInShip.Length;i++) {
            if (amountInShip[i] == 0) {
                rescuePickupInScene.transform.GetChild(0).transform.GetChild(0).transform.GetComponent<RescueMovement>().canFitOnShip = false;
                amountInShip[i++] = 1;
            }
            else {
                rescuePickupInScene.transform.GetChild(0).transform.GetChild(0).transform.GetComponent<RescueMovement>().canFitOnShip = false;
            }
        }
    }

    public void RemovePassenger() {
        for (int i = 0;i < amountInShip.Length;i++) {
            amountInShip[i++] = 0;
        }
    }
}
