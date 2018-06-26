using UnityEngine;

public class Oscilloscope : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve sampleDegredation;
    [SerializeField]
    private AnimationCurve distanceDegredation;

    private int samples = 2048;
    private int resolution;

    private float[] leftSamples;
    private float[] rightSamples;
    private Color[] pixels;

    new private MeshRenderer renderer;
    private AudioSource audioSource;
    private Texture2D texture;

    static float _sqrt2 = Mathf.Sqrt(2);

    private void Awake()
    {
        leftSamples = new float[samples];
        rightSamples = new float[samples];
        pixels = new Color[resolution * resolution];

        renderer = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
        texture = new Texture2D(resolution, resolution);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Trilinear;
        texture.anisoLevel = 16;

        renderer.material.mainTexture = texture;
    }

    private void Update()
    {
        if (Screen.height != resolution)
        {
            resolution = Screen.height;
            pixels = new Color[resolution * resolution];
            texture.Resize(resolution, resolution);
        }

        audioSource.GetOutputData(leftSamples, 0);
        audioSource.GetOutputData(rightSamples, 1);

        Vector2 coordinatesLast = new Vector2(Mathf.RoundToInt((1 + leftSamples[1]) * (resolution - 1) / 2),
                                              Mathf.RoundToInt((1 + rightSamples[1]) * (resolution - 1) / 2));
        for (int i = 1; i < samples; i++)
        {
            Vector2 coordinates = new Vector2(Mathf.RoundToInt((1 + leftSamples[i]) * (resolution - 1) / 2),
                                              Mathf.RoundToInt((1 + rightSamples[i]) * (resolution - 1) / 2));

            //pixels[vertical * size + horizontal] += Color.green * degradationCurve.Evaluate((i + 1f) / samples);

            DrawLine(pixels, (int)coordinatesLast.x, (int)coordinatesLast.y, (int)coordinates.x, (int)coordinates.y,
                     distanceDegredation.Evaluate(Vector2.Distance(coordinates, coordinatesLast) / (resolution * _sqrt2)) * sampleDegredation.Evaluate((i + 1f) / samples));

            coordinatesLast = coordinates;
        }

        texture.SetPixels(pixels);
        texture.Apply();

        pixels = new Color[pixels.Length];
    }

    //Modifed version of http://wiki.unity3d.com/index.php?title=TextureDrawLine
    void DrawLine(Color[] pixels, int x1, int y1, int x2, int y2, float intensity)
    {
        int dy = y2 - y1;
        int dx = x2 - x1;
        int stepx, stepy;

        if (dy < 0) { dy = -dy; stepy = -1; }
        else { stepy = 1; }
        if (dx < 0) { dx = -dx; stepx = -1; }
        else { stepx = 1; }
        dy <<= 1;
        dx <<= 1;

        float fraction = 0;

        IncreasePixel(pixels, y1 * resolution + x1, intensity);
        if (dx > dy)
        {
            fraction = dy - (dx >> 1);
            while (Mathf.Abs(x1 - x2) > 1)
            {
                if (fraction >= 0)
                {
                    y1 += stepy;
                    fraction -= dx;
                }
                x1 += stepx;
                fraction += dy;
                IncreasePixel(pixels, y1 * resolution + x1, intensity);
            }
        }
        else
        {
            fraction = dx - (dy >> 1);
            while (Mathf.Abs(y1 - y2) > 1)
            {
                if (fraction >= 0)
                {
                    x1 += stepx;
                    fraction -= dy;
                }
                y1 += stepy;
                fraction += dx;
                IncreasePixel(pixels, y1 * resolution + x1, intensity);
            }
        }
    }

    void IncreasePixel(Color[] pixels, int index, float intensity)
    {
        pixels[index].g += intensity;

        pixels[index].r += Mathf.Clamp(intensity - 1, 0, float.MaxValue) / 2;
        pixels[index].b += Mathf.Clamp(intensity - 1, 0, float.MaxValue) / 2;

        pixels[index].r = Mathf.Clamp01(pixels[index].r);
        pixels[index].g = Mathf.Clamp01(pixels[index].g);
        pixels[index].b = Mathf.Clamp01(pixels[index].b);
    }
}
