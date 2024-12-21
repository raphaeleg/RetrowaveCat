using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class StoryManager : MonoBehaviour
{
    [Serializable]
    public struct Story
    {
        public Sprite illustration;
        public string text;
        public bool isConnector;
    }
    [SerializeField] private Image panel;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Story[] seq;
    private float fadeDuration = 1.5f;
    private float stayDuration = 3f;
    private Coroutine currentCoroutine = null;

    private void Start()
    {
        StartCoroutine("StorySequence");
    }

    private IEnumerator StorySequence()
    {
        for (int i = 0; i < seq.Length; i++)
        {
            if (i > 0 && seq[i - 1].isConnector) { continue; }

            if (currentCoroutine != null) { StopCoroutine(currentCoroutine); }
            SetStory(i);
            currentCoroutine = StartCoroutine(Fade(0.0f, 1.0f));    // show image

            if (seq[i].isConnector) {
                yield return new WaitForSeconds(fadeDuration);
                if (seq[i+1].text != "") { SetStory(i+1); }
                else { panel.sprite = seq[i+1].illustration; }
                yield return new WaitForSeconds(fadeDuration);
            } else {
                yield return new WaitForSeconds(stayDuration);
            }
            if (currentCoroutine != null) { StopCoroutine(currentCoroutine); }
            currentCoroutine = StartCoroutine(Fade(1.0f, 0.0f));   // hide image
            yield return new WaitForSeconds(stayDuration);
        }
        EventManager.TriggerEvent("LoadMouseHunt",1);
    }

    private void SetStory(int val)
    {
        text.text = seq[val].text;
        panel.sprite = seq[val].illustration;
    }
    private IEnumerator Fade(float start, float end)
    {
        float timerCurrent = 0f;
        float transitionDuration = 0.01f - (fadeDuration * 0.01f);

        while (timerCurrent <= fadeDuration)
        {
            timerCurrent += Time.deltaTime;
            float a = Mathf.Lerp(start, end, timerCurrent / fadeDuration);
            Color c = panel.color;
            panel.color = new Color(c.r, c.g, c.b, a);
            yield return new WaitForSeconds(transitionDuration);
        }
        currentCoroutine = null;
    }
}
