using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRotationController : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] private float torqueZ = 5000000;
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetChild(0).GetComponent<Rigidbody>();
        rb.AddTorque(new Vector3(0, 0, torqueZ));
    }
}
