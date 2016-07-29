using UnityEngine;

public class MicrophoneInput : MonoBehaviour
{
    private AudioSource audioSource;
    private string currentDevice = null;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCurrentMic();
    }

    private void OnEnable()
    {
        StartCurrentMic();
    }

    private void OnDisable()
    {
        StopCurrentMic();
    }

    public void ChangeInput(int device)
    {
        ChangeInput(Microphone.devices[device]);
    }

    public void ChangeInput(string device)
    {
        StopCurrentMic();
        currentDevice = device;
        StartCurrentMic();
    }

    public void StartCurrentMic()
    {
        audioSource.loop = true;
        audioSource.clip = Microphone.Start(currentDevice, true, 5, AudioSettings.outputSampleRate);

        while (Microphone.GetPosition(currentDevice) == 0) ;

        audioSource.Play();
    }

    public void StopCurrentMic()
    {
        if (Microphone.IsRecording(currentDevice))
        {
            Microphone.End(currentDevice);
        }

        audioSource.loop = false;
        audioSource.Stop();
        audioSource.clip = null;
    }
}
