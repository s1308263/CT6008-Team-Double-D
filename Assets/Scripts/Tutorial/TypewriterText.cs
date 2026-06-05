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

    public static event Action AllTextShown;
    public static event Action<char> charactersShown;

    public int tutValues;
    [SerializeField] private float textTimer, maxTextTimer;
    [SerializeField] private GameObject canvas, tutorial, enemySpawnTimer, rescuePlatform;
    public bool canNextText, canStartTextTimer = false;

    private void Awake() {
        text = GetComponent<TMP_Text>();
        tutText = GetComponent<TutorialText>();
        shortDelay = new WaitForSeconds(1 / textSpeed);
        longDelay = new WaitForSeconds(longDelayTime);
        skippingDelay = new WaitForSeconds(1 / (textSpeed * skipSpeed));
        eventDelay = new WaitForSeconds(doneDelay);
        tutorial = gameObject.transform.parent.gameObject;
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

    private IEnumerator Typing() {
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
                        textTimer = 0;
                    }
                    break;
                    case 6:
                    yield return new WaitForSeconds(5);
                    text.SetText("(*RING... RING...*)");
                        enemySpawnTimer.SetActive(true);
                        tutorial.SetActive(false);
                        break;
                    case 7:
                        canStartTextTimer = true;
                        if (canNextText == true) {
                        text.SetText(tutText.tutorialValues[7]);
                        tutValues++;
                        canNextText = false;
                        canStartTextTimer = false;
                        textTimer = 0;
                    }
                    break;
                    case 8:
                    yield return new WaitForSeconds(5);
                    text.SetText("(*RING... RING...*)");
                        rescuePlatform.SetActive(true);
                        tutorial.SetActive(false);
                        break;
                    case 9:
                        canStartTextTimer = true;
                    if (canNextText == true) {
                        text.SetText(tutText.tutorialValues[8]);
                        tutValues++;
                        canNextText = false;
                        canStartTextTimer = false;
                        textTimer = 0;
                    }
                    break;
                    case 10:
                    yield return new WaitForSeconds(5);
                    canvas.GetComponent<Level_Complete>().NextLevel();
                    tutorial.SetActive(false);
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

