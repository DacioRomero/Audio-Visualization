using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource)), DisallowMultipleComponent]
public class Visualizer : ModeChanger
{
    [SerializeField]
    private FFTWindow fftWindow = FFTWindow.BlackmanHarris;
    [SerializeField, Range(0, 1)]
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

        for (int i = 0; i < transformCount; i++)
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
                    grav.position = Vector3.up * Camera.main.transform.position.y;

                    materials[i] = transforms[i].GetComponent<Renderer>().material;
                    materials[i].EnableKeyword("_EMISSION");
                    materials[i].color = transformColor;

                    break;
                case Modes.Particles:
                    transforms[i] = (Instantiate(particlePrefab, Quaternion.Euler(0, ((float)i / transformCount) * 360, 0) * right, particlePrefab.transform.rotation) as GameObject).transform;
                    transforms[i].name = "Particle " + (i + 1);

                    transformColor.a = 0;
                    ParticleSystem.MainModule mainModule = transforms[i].GetComponent<ParticleSystem>().main;
                    mainModule.startColor = transformColor;

                    break;
            }

            transforms[i].parent = transform;
        }
    }

    private void Update()
    {
        float[] samples = new float[8192];
        audioSource.GetSpectrumData(samples, 0, fftWindow);

        float loHz = 20, hiHz = 0;
        for (int i = 0; i < transforms.Length; i++)
        {
            hiHz = 440 * Mathf.Pow(_2root12, (i + 1) * noteStepsPerCube - 58);
            float volume = GetMaxVolInRange(ref samples, loHz, hiHz);

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
                ParticleSystem.MainModule mainModule = particleSystem.main;
                ParticleSystem.EmissionModule emissionModule = particleSystem.emission;

                ParticleSystem.MinMaxGradient startColor = mainModule.startColor;
                Color color = startColor.color;
                color.a = Mathf.Lerp(color.a, volume * (mainModule.startSpeed.constant / emissionModule.rateOverTime.constantMax), Time.deltaTime * (1 / smoothValue));
                startColor.color = color;
                mainModule.startColor = startColor;
            }

            if (mode != Modes.Particles)
            {
                materials[i].SetColor("_EmissionColor", Color.Lerp(materials[i].GetColor("_EmissionColor"), materials[i].color * volume, Time.deltaTime * (1 / smoothValue)));
            }

            loHz = hiHz;
        }
    }

    public override List<Dropdown.OptionData> GetModes()
    {
        List<Dropdown.OptionData> modes = new List<Dropdown.OptionData>();

        foreach (string name in System.Enum.GetNames(typeof(Modes)))
        {
            modes.Add(new Dropdown.OptionData(name));
        }

        return modes;
    }

    public override void SetMode(Dropdown dropdown)
    {
        mode = (Modes)dropdown.value;

        for (int i = 0; i < transforms.Length; i++)
        {
            Destroy(transforms[i].gameObject);
        }

        Start();
    }

    private float GetMaxVolInRange(ref float[] samples, float lowHz, float highHz)
    {
        lowHz = Mathf.Clamp(lowHz, 0, AudioSettings.outputSampleRate);
        highHz = Mathf.Clamp(highHz, lowHz, AudioSettings.outputSampleRate);

        int first = Mathf.FloorToInt(lowHz * samples.Length / AudioSettings.outputSampleRate);
        int last = Mathf.FloorToInt(highHz * samples.Length / AudioSettings.outputSampleRate);

        float max = 0;

        for (int i = first; i <= last; i++)
        {
            if (samples[i] > max) max = samples[i];
        }

        return max;
    }
}
