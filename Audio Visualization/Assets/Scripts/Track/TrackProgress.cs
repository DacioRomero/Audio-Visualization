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
                rectTransform.localScale = new Vector3((float)audioSource.timeSamples / audioSource.clip.samples, 1, 1);
            }

            else
            {
                rectTransform.localScale = Vector3.zero;
            }
        }
    }

    public void TrackTimeAtMouse()
    {
        audioSource.timeSamples = Mathf.Clamp(Mathf.FloorToInt(Mathf.Clamp01(Input.mousePosition.x / Screen.width) * audioSource.clip.samples), 0, audioSource.clip.samples - 1);
    }
}
