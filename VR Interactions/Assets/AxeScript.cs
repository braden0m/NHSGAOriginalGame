using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeScript : MonoBehaviour
{
    public float createdForce;
    private Rigidbody rb;

    private Vector3 customVelocity;
    private Vector3 customAngularVelocity;
    private Vector3 lastPosition;
    private Vector3 currentPosition;
    private Vector3 lastRotation;
    private Vector3 currentRotation;

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
        currentPosition = transform.position;
        lastRotation = transform.rotation.eulerAngles;
        currentRotation = transform.rotation.eulerAngles;


        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = transform.position;
        currentRotation = transform.rotation.eulerAngles;

        customVelocity = currentPosition - lastPosition;
        customAngularVelocity = currentRotation - lastRotation;

        createdForce = Mathf.Abs(customVelocity.magnitude * 2 + customAngularVelocity.magnitude);
        print(rb.velocity);

        lastPosition = currentPosition;
        lastRotation = transform.rotation.eulerAngles;
    }
}
