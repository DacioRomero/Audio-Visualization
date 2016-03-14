using UnityEngine;

public delegate void PlayCurrentTrackListener(string text);

[RequireComponent(typeof(AudioSource))]
public class TrackShuffle : MonoBehaviour
{
    [SerializeField]
    private string trackDirectory = "Tracks";
    private AudioClip[] tracks;
    private int[] shuffledList;
    private int currentTrack;
    private AudioSource audioSource;
    private int timeSamples;
    private bool paused;
    public event PlayCurrentTrackListener OnPlayCurrentTrack;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        tracks = Resources.LoadAll<AudioClip>(trackDirectory);
        Shuffle();
    }

    private void Start()
    {
        PlayCurrentTrack();
    }

    private void Update()
    {
        if (!audioSource.isPlaying && !paused)
        {
            NextTrack();
        }
    }

    private void OnDisable()
    {
        StopCurrentTrack(out timeSamples);
        paused = false;
    }

    private void OnEnable()
    {
        PlayCurrentTrack(timeSamples);
    }

    public void Shuffle()
    {
        shuffledList = new int[tracks.Length];

        for (int i = 0; i < shuffledList.Length; i++)
        {
            shuffledList[i] = i;
        }

        for (int i = 0; i < shuffledList.Length; i++)
        {
            int tmp = shuffledList[i];
            int r = Random.Range(i, shuffledList.Length);
            shuffledList[i] = shuffledList[r];
            shuffledList[r] = tmp;
        }

        currentTrack = 0;
    }

    public void NextTrack()
    {
        StopCurrentTrack();
        currentTrack += 1;
        currentTrack %= tracks.Length;
        PlayCurrentTrack();
    }

    public void LastTrack()
    {
        StopCurrentTrack();
        currentTrack -= 1;
        currentTrack %= tracks.Length;
        if (currentTrack < 0)
        {
            currentTrack += tracks.Length;
        }
        PlayCurrentTrack();
    }

    private void StopCurrentTrack(out int sample)
    {
        sample = audioSource.timeSamples;
        StopCurrentTrack();
    }

    private void StopCurrentTrack()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        audioSource.clip = null;
        audioSource.timeSamples = 0;
    }

    public void PlayCurrentTrack()
    {
        PlayCurrentTrack(0);
    }

    public void PlayCurrentTrack(int sample)
    {
        audioSource.clip = tracks[shuffledList[currentTrack]];
        audioSource.timeSamples = sample;
        audioSource.Play();
        if (OnPlayCurrentTrack != null)
        {
            OnPlayCurrentTrack(audioSource.clip.name);
        }
    }

    public void Pause()
    {
        paused = true;
        audioSource.Pause();
    }

    public void UnPause()
    {
        paused = false;
        audioSource.UnPause();
    }
}
