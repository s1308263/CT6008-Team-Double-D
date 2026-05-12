using UnityEngine;
using UnityEngine.UI;

public class Player_Rescue : MonoBehaviour {

    public int[] canFitInShip;
    public int totalNeedRescuing;
    [SerializeField] private int shipCapacity, maxShipCapacity;

    [SerializeField] private GameObject everyoneRescuedText, rescueFollow;

    public Sprite emptySeat;
    public Sprite fullSeat;
    public Image[] seats;

    public GameObject[] followArray;
    public GameObject rescuePickupInScene;

    float rescuedTimer;
    bool canShowRescueText;

    private void Awake() {
        //RANDOM SPAWN PLATFORMS HERE

        ////////////////////////////
        //Remove for player class
        shipCapacity = 1;
        maxShipCapacity = 1;
        ////////////////////////////

        if(shipCapacity >= maxShipCapacity) {
            shipCapacity = maxShipCapacity;
        }
        canFitInShip = new int[shipCapacity];
        followArray = new GameObject[shipCapacity];
        canShowRescueText = true;
    }

    private void Update()
    {
        if (totalNeedRescuing <= 0 && canShowRescueText == true)
        {
            //Show all rescued text here
            totalNeedRescuing = 0;
            everyoneRescuedText.SetActive(true);
            rescuedTimer += 1 * Time.deltaTime;
            if (rescuedTimer >= 10)
            {
                everyoneRescuedText.SetActive(false);
                canShowRescueText = false;
            }
            Debug.Log("Everyone Rescued");
        }
    }

    public void AddCapacity() {
        if (shipCapacity < maxShipCapacity) {
            shipCapacity++;
        }
    }

    public void Rescued() {
        totalNeedRescuing--;
    }

    public void AddPassenger() {
        for (int i = 0;i < canFitInShip.Length;i++) {
            if (canFitInShip[i] == 0) {
                canFitInShip[i++] = 1;
            }
        }
    }

    public void RemovePassenger() {
        for (int i = 0;i < canFitInShip.Length;i++) {
            if(canFitInShip[i] == 1)
            canFitInShip[i++] = 0;
        }
        for(int j = 0; j < followArray.Length;j++) {
            if (followArray[j] != null) {
                Destroy(followArray[j]);
                followArray[j++] = null;
            }
        }
    }

    public void CapacityCheck() {
        for (int i = 0;i < canFitInShip.Length;i++) {
            if (canFitInShip[i] == 0) {
                rescuePickupInScene.transform.GetChild(0).transform.GetComponent<RescueMovement>().canFitOnShip = true;
            }
            else {
                rescuePickupInScene.transform.GetChild(0).transform.GetComponent<RescueMovement>().canFitOnShip = false;
            }
        }
    }
}
