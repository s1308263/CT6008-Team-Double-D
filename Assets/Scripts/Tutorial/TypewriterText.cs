using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using TMPro;
using Object = UnityEngine.Object;

[RequireComponent(typeof(TMP_Text))]
public class TypewriterText : MonoBehaviour {

    private TMP_Text text;
    private TutorialText tutText;
    private EnemySpawnScript enemySpawnScript;

    //Functionality
    private int textIndex;
    private Coroutine coroutine;
    private bool isreadyForNextText = true;

    private WaitForSeconds shortDelay;
    private WaitForSeconds longDelay;

    [Header("Typewriter Settings")]
    [SerializeField] private float textSpeed = 20;
    [SerializeField] private float longDelayTime = 0.5f;

    [Header("Skipping")]
    public bool isSkipping { get; private set; }
    private WaitForSeconds skippingDelay;

    [Header("Skip Options")]
    [SerializeField] private bool fastSkip;
    [SerializeField] [Min(1)] private int skipSpeed = 5;

    [Header("Event Options")]
    [SerializeField]
    [Range(0.1f, 0.5f)] private float doneDelay = 0.25f;
    private WaitForSeconds eventDelay;
    [SerializeField] private GameObject enemySpawnTimer;

    public static event Action AllTextShown;
    public static event Action<char> charactersShown;

    private int tutValues;
    [SerializeField] private float textTimer, maxTextTimer;
    public bool canNextText, canStartTextTimer = false;

    private void Awake() {
        text = GetComponent<TMP_Text>();
        tutText = GetComponent<TutorialText>();
        enemySpawnScript = GetComponent<EnemySpawnScript>();
        shortDelay = new WaitForSeconds(1 / textSpeed);
        longDelay = new WaitForSeconds(longDelayTime);
        skippingDelay = new WaitForSeconds(1 / (textSpeed * skipSpeed));
        eventDelay = new WaitForSeconds(doneDelay);
    }

    private void Start() {
        text.SetText(tutText.tutorialValues[0]);
        tutValues = 0;
    }

    private void Update() {
        if (canStartTextTimer == true) {
            textTimer += 1 * Time.deltaTime;
            if (textTimer >= maxTextTimer) {
                canNextText = true;
                textTimer = 0;
                StartCoroutine(Typing());
            }
        }
    }

    private void OnEnable() {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ReadyNextText);
    }

    private void OnDisable() {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ReadyNextText);
    }

    public void ReadyNextText(Object obj) {
        if (!isreadyForNextText) {
            return;
        }
        isSkipping = false;
        isreadyForNextText = false;

        if (coroutine != null) {
            StopCoroutine(coroutine);
        }

        text.maxVisibleCharacters = 0;
        textIndex = 0;

        coroutine = StartCoroutine(Typing());
    }

    public void OnSkip(InputAction.CallbackContext context) {
        if (context.performed) {
            if (text.maxVisibleCharacters != text.textInfo.characterCount - 1) {
                SkipText();
            }
        }
    }

    private void SkipText() {
        if (isSkipping) {
            return;
        }
        isSkipping = true;
        if (!fastSkip) {
            StartCoroutine(SkipSpeedReset());
            return;
        }
        StopCoroutine(coroutine);
        text.maxVisibleCharacters = text.textInfo.characterCount;
        AllTextShown?.Invoke();
    }

    public IEnumerator Typing() {
        TMP_TextInfo textInfo = text.textInfo;

        while (textIndex < textInfo.characterCount + 1) {
            var lastIndex = textInfo.characterCount - 1;

            if (textIndex == lastIndex) {
                text.maxVisibleCharacters++;
                yield return eventDelay;
                AllTextShown?.Invoke();
                isreadyForNextText = true;
                switch (tutValues) {
                    case 0:
                    canStartTextTimer = true;
                    if (canNextText == true) {
                        text.SetText(tutText.tutorialValues[1]);
                        tutValues++;
                        canNextText = false;
                        canStartTextTimer = false;
                        textTimer = 0;
                    }
                    break;
                    case 1:
                    canStartTextTimer = true;
                    if (canNextText == true) {
                        text.SetText(tutText.tutorialValues[2]);
                        tutValues++;
                        canNextText = false;
                        canStartTextTimer = false;
                        textTimer = 0;
                    }
                    break;
                    case 2:
                    canStartTextTimer = true;
                    if (canNextText == true) {
                        text.SetText(tutText.tutorialValues[3]);
                        tutValues++;
                        canNextText = false;
                        canStartTextTimer = false;
                        textTimer = 0;
                    }
                    break;
                    case 3:
                    canStartTextTimer = true;
                    if (canNextText == true) {
                        text.SetText(tutText.tutorialValues[4]);
                        tutValues++;
                        canNextText = false;
                        canStartTextTimer = false;
                        textTimer = 0;
                    }
                    break;
                    case 4:
                    canStartTextTimer = true;
                    if (canNextText == true) {
                        text.SetText(tutText.tutorialValues[5]);
                        tutValues++;
                        canNextText = false;
                        canStartTextTimer = false;
                        textTimer = 0;
                    }
                    break;
                    case 5:
                    canStartTextTimer = true;
                    if (canNextText == true) {
                        text.SetText(tutText.tutorialValues[6]);
                        tutValues++;
                        canNextText = false;
                        canStartTextTimer = false;
                        enemySpawnTimer.SetActive(true);
                        textTimer = 0;
                    }
                    break;
                    case 6:
                    //canStartTextTimer = true;
                    if (canNextText == true) {
                        text.SetText(tutText.tutorialValues[7]);
                        tutValues++;
                        canNextText = false;
                        canStartTextTimer = false;
                        textTimer = 0;
                    }
                    break;
                    case 7:
                    canStartTextTimer = true;
                    if (canNextText == true) {
                        text.SetText(tutText.tutorialValues[8]);
                        tutValues++;
                        canNextText = false;
                        canStartTextTimer = false;
                        textTimer = 0;
                    }
                    break;
                }
                yield break;
            }

            char character = textInfo.characterInfo[textIndex].character;
            text.maxVisibleCharacters++;

            if (!isSkipping && (character == '?' || character == '.' || character == ',' ||  character == '!')) {
                yield return longDelay;
            }
            else {
                yield return isSkipping ? skippingDelay : shortDelay;
            }
            AllTextShown?.Invoke();
            textIndex++;
        }
    }

    private IEnumerator SkipSpeedReset() {
        yield return new WaitUntil(() => text.maxVisibleCharacters == text.textInfo.characterCount - 1);
        isSkipping = false;
    }
}

