using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class TrackName : MonoBehaviour
{
    [SerializeField]
    private TrackShuffle shuffle;
    [SerializeField]
    AnimationCurve fadeCurve;
    private Text textObject;

    private void Awake()
    {
        textObject = GetComponent<Text>();
        if (shuffle)
        {
            shuffle.OnPlayCurrentTrack += DisplayTrack;
        }

        else
        {
            AudioSource audioSource = FindObjectOfType<AudioSource>();

            if (audioSource)
            {
                DisplayTrack(audioSource.clip.name);
            }
        }
    }

    private void DisplayTrack(string text)
    {
        StopAllCoroutines();
        StartCoroutine(Fade(text));
    }

    IEnumerator Fade(string text)
    {
        float currentTime = 0;
        float time = fadeCurve.keys[fadeCurve.keys.Length - 1].time;
        textObject.text = text;

        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            Color textColor = textObject.color;
            textColor.a = fadeCurve.Evaluate(currentTime);
            textObject.color = textColor;
            yield return null;
        }
    }
}
