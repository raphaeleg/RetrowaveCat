using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A signleton class that passes data to AudioManager based on a triggered Event.
/// </summary>
public class FMODEvents : MonoBehaviour
{
    public static FMODEvents Instance { get; private set; }

    #region EventReference Variables
    [field: Header("Music")]
    [field: SerializeField] public EventReference BG { get; private set; }

    [field: Header("SFX")]
    [field: SerializeField] public EventReference ButtonClick { get; private set; }

    [field: Header("Gameplay")]
    [field: SerializeField] public EventReference Meow { get; private set; }
    [field: SerializeField] public EventReference Mouse { get; private set; }
    [field: SerializeField] public EventReference Attack { get; private set; }
    #endregion

    #region Event Listeners
    private Dictionary<string, Action<int>> SubscribedEvents = new();
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        SubscribedEvents = new() {
                { "SFX_Button", SFX_Click },
                { "Meow", SFX_Meow },
                { "SpawnMouse", SFX_Mouse },
                { "CaughtMouse", SFX_Attack },

                { "ChangeMusicArea", ChangeArea },
            };
    }

    private void OnEnable()
    {
        StartCoroutine("DelayedSubscription");
    }
    private IEnumerator DelayedSubscription()
    {
        yield return new WaitForSeconds(0.0001f);
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

    private void Play(EventReference eventRef)
    {
        AudioManager.Instance.PlayOneShot(eventRef);
    }
    private void SFX_Click(int val = 0)
    {
        Play(ButtonClick);
    }
    private void SFX_Meow(int val = 0)
    {
        Play(Meow);
    }
    private void SFX_Mouse(int val = 0)
    {
        Play(Mouse);
    }
    private void SFX_Attack(int val = 0)
    {
        Play(Attack);
    }
    public void ChangeArea(int area)
    {
        //if (area == 2)
        //{
        //    AudioManager.Instance.CleanGameInstances();
        //}
        AudioManager.Instance.SetMusicArea((Audio_MusicArea)area);
    }
}