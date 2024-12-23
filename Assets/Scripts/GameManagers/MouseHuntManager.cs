using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using System.Collections;

public class MouseHuntManager : MonoBehaviour
{
    [Header("Mouse Minigame")]
    [SerializeField] private GameObject mousePrefab;
    [SerializeField] private Transform player;
    [SerializeField] private Transform mouseSpawnPoints;
    [SerializeField] private Transform mouseParent;
    [SerializeField] private float spawnFreq = 2f;
    [SerializeField] private float time = 15f;
    [SerializeField] private int score = 0;

    [Header("After Minigame")]
    private bool pickedSpot = false;
    [SerializeField] private TMP_Text instructions;
    [SerializeField] private GameObject humanPrefab;

    [Header("Connectors")]
    [SerializeField] GameObject PauseScreen;
    [SerializeField] Transform GameUI;
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text scoreText;

    #region EventManager
    private Dictionary<string, Action<int>> SubscribedEvents;

    private void Awake()
    {
        SubscribedEvents = new()
        {
            { "CaughtMouse", AddScore },
            { "Sit", ShowCorpses },
        };
    }
    private void OnEnable()
    {
        foreach (var pair in SubscribedEvents)
        {
            EventManager.StartListening(pair.Key, pair.Value);
        }
    }
    private void OnDisable()
    {
        foreach (var pair in SubscribedEvents)
        {
            EventManager.StopListening(pair.Key, pair.Value);
        }
    }
    #endregion

    private void Start()
    {
        StartCoroutine("MinigameTimer");
        EventManager.TriggerEvent("ChangeMusicArea", 2);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseScreen.SetActive(true);
        }
    }

    private IEnumerator MinigameTimer()
    {
        while (time >= 0) {
            yield return new WaitForSeconds(1);
            time--;
            UpdateTimeText();
            if (time <= 10) { SpawnMouse(); }
            if (time % spawnFreq == 0)
            {
                SpawnMouse();
            }
        }
        EndMinigame();
    }
    private void UpdateTimeText()
    {
        if (time < 0) { return; }

        string t = time.ToString();
        if (t.Length == 1) { t = "0" + t; }
        timerText.text = "00:" + t;
    }

    private void SpawnMouse()
    {
        var mouse = Instantiate(mousePrefab, mouseParent);
        int randPoint = UnityEngine.Random.Range(0, mouseSpawnPoints.childCount);
        mouse.transform.position = mouseSpawnPoints.GetChild(randPoint).transform.position;
        EventManager.TriggerEvent("SpawnMouse", 1);
    }

    private void AddScore(int val)
    {
        if (time < 0 || pickedSpot) { return; }
        score += val;
        scoreText.text = "x"+score.ToString()+" caught";
    }
    private void ClearMouse()
    {
        foreach (Transform child in mouseParent)
        {
            Destroy(child.gameObject);
        }
    }
    private void EndMinigame()
    {
        ClearMouse();
        EventManager.TriggerEvent("ChangeMusicArea", 1);
        //Instantiate(humanPrefab); //TODO: make human script

        foreach (Transform child in GameUI)
        {
            GameObject childGO = child.gameObject;
            childGO.SetActive(!childGO.activeSelf);
        }
    }
    private void ShowCorpses(int isFacingRight)
    {
        if (time > 0 || pickedSpot) { return; }
        pickedSpot = true;

        ClearMouse();
        Vector3 pos = player.position;
        pos.x += 5f * isFacingRight;
        mouseParent.position = pos;

        for (int i = 0; i < score; i++) {
            var mouse = Instantiate(mousePrefab, mouseParent);
            mouse.GetComponent<Mouse>().speed = 0f;
        }
        instructions.text = "";
        StartCoroutine("EndScene");
    }

    private IEnumerator EndScene()
    {
        yield return new WaitForSeconds(2f);
        EventManager.TriggerEvent("EndMouseHunt", 1);
        yield return new WaitForSeconds(1f);
        EventManager.TriggerEvent("LoadMainMenu", 1);
    }
}
