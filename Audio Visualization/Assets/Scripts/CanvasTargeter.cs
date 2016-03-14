using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class CanvasTargeter : MonoBehaviour
{
    private void Awake()
    {
        if (Screen.fullScreen)
        {
            Canvas canvas = GetComponent<Canvas>();
            canvas.targetDisplay = PlayerPrefs.GetInt("UnitySelectMonitor");
        }

        Destroy(this);
    }
}
