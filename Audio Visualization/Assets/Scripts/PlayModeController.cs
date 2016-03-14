using UnityEngine;

public class PlayModeController : MonoBehaviour
{
    [SerializeField]
    private bool defaultMicrophone;
    [SerializeField]
    private MonoBehaviour[] microphoneScripts;
    [SerializeField]
    private MonoBehaviour[] shuffleScripts;
    [SerializeField]
    private GameObject[] microphoneObjects;
    [SerializeField]
    private GameObject[] shuffleObjects;

    private void Awake()
    {
        SetMode(defaultMicrophone);
    }

    public void SetMode(bool microphone)
    {
        if (microphone)
        {
            foreach (GameObject _object in shuffleObjects)
            {
                _object.SetActive(!microphone);
            }

            foreach (GameObject _object in microphoneObjects)
            {
                _object.SetActive(microphone);
            }

            foreach (MonoBehaviour script in shuffleScripts)
            {
                script.enabled = !microphone;
            }

            foreach (MonoBehaviour script in microphoneScripts)
            {
                script.enabled = microphone;
            }
        }

        else
        {
            foreach (GameObject _object in microphoneObjects)
            {
                _object.SetActive(microphone);
            }

            foreach (GameObject _object in shuffleObjects)
            {
                _object.SetActive(!microphone);
            }

            foreach (MonoBehaviour script in microphoneScripts)
            {
                script.enabled = microphone;
            }

            foreach (MonoBehaviour script in shuffleScripts)
            {
                script.enabled = !microphone;
            }
        }
    }
}
