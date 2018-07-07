using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Gravitate : MonoBehaviour
{
    public Vector3 position = Vector3.zero;

    new private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
    }

    private void FixedUpdate()
    {
        rigidbody.velocity += (transform.position - position).normalized * Physics.gravity.y;
        rigidbody.mass = transform.localScale.sqrMagnitude;
    }
}
