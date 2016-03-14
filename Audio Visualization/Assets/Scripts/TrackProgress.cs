using UnityEngine;

public class TrackProgress : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private RectTransform rectTransform;

    private void LateUpdate()
    {
        if (rectTransform && audioSource)
        {
            if (audioSource.clip)
            {
                rectTransform.sizeDelta = new Vector2(((float)audioSource.timeSamples / audioSource.clip.samples) * Screen.width, 10);
            }

            else
            {
                rectTransform.sizeDelta = Vector2.zero;
            }
        }
    }

    public void TrackTimeAtMouse()
    {
        audioSource.timeSamples = Mathf.Clamp(Mathf.FloorToInt(Mathf.Clamp01(Input.mousePosition.x / Screen.width) * audioSource.clip.samples), 0, audioSource.clip.samples - 1);
    }
}
