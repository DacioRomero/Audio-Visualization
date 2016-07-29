using UnityEngine;

public class Oscilloscope : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve degradationCurve;

    private int size = 1024;
    private int samples = 8192;

    private float[] leftSamples;
    private float[] rightSamples;
    private Color[] colors;

    private MeshRenderer renderer;
    private AudioSource audioSource;
    private Texture2D texture;

    private void Awake()
    {
        leftSamples = new float[samples];
        rightSamples = new float[samples];
        colors = new Color[size * size];

        renderer = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
        texture = new Texture2D(size, size);

        renderer.material.mainTexture = texture;
    }

    private void Update()
    {
        audioSource.GetOutputData(leftSamples, 0);
        audioSource.GetOutputData(rightSamples, 1);

        for(int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.black;
        }

        for (int i = 0; i < samples; i++)
        {
            int horizontal = Mathf.RoundToInt((1 + leftSamples[i]) * (size - 1) / 2);
            int vertical = Mathf.RoundToInt((1 + rightSamples[i]) * (size - 1) / 2);
            colors[vertical * size + horizontal] += Color.green * degradationCurve.Evaluate((i + 1f) / samples);
        }

        texture.SetPixels(colors);
        texture.Apply();
    }
}
