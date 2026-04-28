using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class OffscreenIndicator : MonoBehaviour {

    public GameObject[] playerTargets;
    public GameObject indicatorPrefab;
    [SerializeField] GameObject playerRotator, playerInScene;

    private SpriteRenderer spriteRend;
    private float _width;
    private float _height;

    private Camera cam;
    private Dictionary<GameObject, GameObject> targetIndicators = new Dictionary<GameObject, GameObject>();


    private void Awake() {
        cam = Camera.main;
        spriteRend = indicatorPrefab.GetComponent<SpriteRenderer>();

        var bounds = spriteRend.bounds;
        _width = bounds.size.x / 2f;
        _height = bounds.size.y / 2f;

        foreach (var target in playerTargets) {
            var indicator = Instantiate(indicatorPrefab, transform);
            indicator.SetActive(false);
            targetIndicators.Add(target, indicator);
            if (playerRotator == null) {
                playerRotator = indicator.transform.GetChild(0).transform.gameObject;
            }
        }
    }

    private void Update() {

        foreach (KeyValuePair<GameObject, GameObject> entry in targetIndicators) {
            var target = entry.Key;
            var indicator = entry.Value;
            if (target != null) {
                UpdateTarget(target, indicator);
            }
        }
    }

    private void UpdateTarget(GameObject target, GameObject indicator) {
        var screenPos = cam.WorldToViewportPoint(target.transform.position);
        bool isOffScreen = screenPos.x <= 0 || screenPos.x >= 1 || screenPos.y <= 0 || screenPos.y >= 1;
        if (isOffScreen) {
            indicator.SetActive(true);
            playerRotator.SetActive(true);
            var spriteSize = cam.WorldToViewportPoint(new Vector3(_width, _height, 0)) - cam.WorldToViewportPoint(Vector3.zero);

            screenPos.x = Mathf.Clamp(screenPos.x, spriteSize.x, 1 - spriteSize.x);
            screenPos.y = Mathf.Clamp(screenPos.y, spriteSize.y, 1 - spriteSize.y);

            var worldPos = cam.ViewportToWorldPoint(screenPos);
            worldPos.z = 0;
            indicator.transform.position = worldPos;

            Vector3 direction = target.transform.position - indicator.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            indicator.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            playerRotator.transform.rotation = playerInScene.transform.rotation;
        }
        else { 
            indicator.SetActive(false);
            playerRotator.SetActive(false);
        }
    }
}
