using UnityEngine;
using FireClaw.Audio;

[RequireComponent(typeof(AudioSource))]
public class Visualizer : MonoBehaviour
{
    [SerializeField]
    private FFTWindow fftWindow = FFTWindow.BlackmanHarris;
    [SerializeField, Range(0f, 1f)]
    private float smoothValue = 0;
    [SerializeField]
    private float scale = 4;
    [SerializeField, Range(1, 64)]
    private int noteStepsPerCube = 2;
    
    [HideInInspector]
    public GameObject particlePrefab;

    private AudioSource audioSource;
    private Transform[] transforms;
    private Material[] materials;

    static float _2root12 = Mathf.Pow(2, 1f / 12);

    public enum Modes
    {
        Prisms,
        Spheres,
        Particles
    }

    [SerializeField]
    private Modes mode;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    private void Start()
    {
        int transformCount = Mathf.CeilToInt((Mathf.Log(AudioSettings.outputSampleRate / 880, _2root12) + 58) / noteStepsPerCube);
        transforms = new Transform[transformCount];
        materials = new Material[transformCount];
        Vector3 right = Vector3.right * transformCount;
        Vector3 gravPos = Vector3.up * Camera.main.transform.position.y;

        for (short i = 0; i < transformCount; i++)
        {
            Color transformColor = Color.Lerp(Color.blue, Color.red, (float)i / (transformCount - 1));
            switch (mode)
            {
                case Modes.Prisms:
                    transforms[i] = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
                    Destroy(transforms[i].gameObject.GetComponent<Collider>());
                    transforms[i].position = Quaternion.Euler(0, ((float)i / transformCount) * 360, 0) * right;
                    transforms[i].name = "Prism " + (i + 1);
                    Renderer rend = transforms[i].GetComponent<Renderer>();
                    rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    rend.receiveShadows = false;
                    materials[i] = rend.material;
                    materials[i].EnableKeyword("_EMISSION");
                    materials[i].color = transformColor;
                    break;
                case Modes.Spheres:
                    transforms[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
                    transforms[i].position = Random.insideUnitSphere * scale;
                    transforms[i].name = "Sphere " + (i + 1);
                    Gravitate grav = transforms[i].gameObject.AddComponent<Gravitate>();
                    grav.position = gravPos;
                    materials[i] = transforms[i].GetComponent<Renderer>().material;
                    materials[i].EnableKeyword("_EMISSION");
                    materials[i].color = transformColor;
                    break;
                case Modes.Particles:
                    transforms[i] = (Instantiate(particlePrefab, Quaternion.Euler(0, ((float)i / transformCount) * 360, 0) * right, particlePrefab.transform.rotation) as GameObject).transform;
                    transforms[i].name = "Particle " + (i + 1);
                    transformColor.a = 0;
                    transforms[i].GetComponent<ParticleSystem>().startColor = transformColor;
                    break;
            }
            transforms[i].parent = transform;
        }
    }

    private void Update()
    {
        float[] samples = new float[8192];
        audioSource.GetSpectrumData(samples, 0, fftWindow);

        float loHz, hiHz = 0;
        for (short i = 0; i < transforms.Length; i++)
        {
            loHz = hiHz;
            hiHz = 440 * Mathf.Pow(_2root12, (i + 1) * noteStepsPerCube - 58);
            float volume = GetRangeVolume(ref samples, loHz, hiHz) * 8;

            if (mode == Modes.Prisms)
            {
                Vector3 localScale = transforms[i].localScale;
                localScale.y = Mathf.Lerp(localScale.y, volume * scale, Time.deltaTime * (1 / smoothValue));
                transforms[i].localScale = localScale;

                Vector3 position = transforms[i].position;
                position.y = localScale.y / 2;
                transforms[i].position = position;
            }

            else if (mode == Modes.Spheres)
            {
                transforms[i].localScale = Vector3.Lerp(transforms[i].localScale, Vector3.one * volume * scale, Time.deltaTime * (1 / smoothValue));
            }

            else if (mode == Modes.Particles)
            {
                ParticleSystem particleSystem = transforms[i].GetComponent<ParticleSystem>();
                Color color = particleSystem.startColor;
                color.a = Mathf.Lerp(color.a, volume * (particleSystem.startSpeed / particleSystem.emission.rate.constantMax), Time.deltaTime * (1 / smoothValue));
                transforms[i].GetComponent<ParticleSystem>().startColor = color;
            }

            if (mode != Modes.Particles)
            {
                materials[i].SetColor("_EmissionColor", Color.Lerp(materials[i].GetColor("_EmissionColor"), materials[i].color * volume, Time.deltaTime * (1 / smoothValue)));
            }
        }
    }

    public void SetMode(int _mode)
    {
        mode = (Modes)_mode;
        for(short i = 0; i < transforms.Length; i++)
        {
            Destroy(transforms[i].gameObject);
        }

        Start();
    }

    private float GetRangeVolume(ref float[] samples, float lowHz, float highHz)
    {
        float max = AudioSettings.outputSampleRate;

        lowHz = Mathf.Clamp(lowHz, 0, max);
        highHz = Mathf.Clamp(highHz, lowHz, max);

        int first = Mathf.FloorToInt(lowHz * samples.Length / max);
        int last = Mathf.FloorToInt(highHz * samples.Length / max);

        float[] dBs = new float[last - first + 1];

        for (int i = first; i <= last; i++)
        {
            dBs[i - first] = AudioUnitConversions.RelToDB(samples[i]);
        }

        float volume = AudioUnitConversions.DBToRel(AudioUnitOperations.AddDBs(dBs));

        if (volume > 1)
        {
            Debug.Log("Range " + lowHz + " - " + highHz + " returned value greater than one: " + volume);
        }

        return volume;
    }
}
