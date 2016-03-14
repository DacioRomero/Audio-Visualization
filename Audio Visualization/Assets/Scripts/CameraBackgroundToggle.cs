using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraBackgroundToggle : MonoBehaviour
{
    [SerializeField]
    private Color colorTrue;
    [SerializeField]
    private Color colorFalse;

    private Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    public void Toggle(bool value)
    {
        camera.backgroundColor = value ? colorTrue : colorFalse;
    }
}
