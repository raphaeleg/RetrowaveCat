using UnityEngine;

public class Button : MonoBehaviour
{
    public void PlaySFXClick()
    {
        EventManager.TriggerEvent("SFX_Button", 1);
    }
}
