using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Gravitate : MonoBehaviour
{
    public Vector3 position = Vector3.zero;

    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
    }

    private void FixedUpdate()
    {
        rigidbody.AddForce((transform.position - position).normalized * Physics.gravity.y * 4, ForceMode.Acceleration);
        rigidbody.mass = transform.localScale.x;
    }
}
