using UnityEngine;

public class RotatePoint : MonoBehaviour
{
    [SerializeField]
    private Vector3 point = Vector3.zero;
    [SerializeField]
    private float degrees = 30;

    private void Update()
    {
        transform.RotateAround(point, Vector3.up, degrees * Time.deltaTime);
    }
}
