using UnityEngine;
using UnityEngine.Audio;
using FireClaw.Audio;

public class AudioMixerController : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer;

    public void SetMasterVolume(float volume)
    {
        mixer.SetFloat("masterVol", AudioUnitConversions.RelToDB(volume));
    }

    public void SetMusicVolume(float volume)
    {
        mixer.SetFloat("musicVol", Mathf.Clamp(AudioUnitConversions.RelToDB(volume), float.MinValue, float.MaxValue));
    }
}
