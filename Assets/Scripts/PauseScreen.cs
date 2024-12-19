using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    private void OnEnable()
    {
        SetPause(true);
    }
    private void OnDisable()
    {
        SetPause(false);
    }
    private void SetPause(bool val)
    {
        Time.timeScale = val ? 0.0f : 1.0f;
    }
}
