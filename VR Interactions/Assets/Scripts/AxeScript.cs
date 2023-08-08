using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeScript : MonoBehaviour
{
    public float createdForce;
    private Rigidbody rb;
    private BoxCollider collider;
    private ParticleSystem thisParticleSystem;

    [SerializeField] private AudioClip[] breakClips;
    private AudioSource breakSound;

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

        collider = GetComponent<BoxCollider>();
        thisParticleSystem = GetComponentInChildren<ParticleSystem>();
        breakSound = GetComponent<AudioSource>();

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = transform.position;
        currentRotation = transform.rotation.eulerAngles;

        customVelocity = currentPosition - lastPosition;
        customAngularVelocity = currentRotation - lastRotation;

        createdForce = Mathf.Abs(customVelocity.magnitude * 3 + customAngularVelocity.magnitude);

        lastPosition = currentPosition;
        lastRotation = transform.rotation.eulerAngles;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Breakable") && collision.gameObject.GetComponentInChildren<ParticleSystem>())
        {
            ParticleSystem targetParticle = collision.gameObject.GetComponentInChildren<ParticleSystem>();
            Material targetMaterial = targetParticle.GetComponentInChildren<Renderer>().material;

            thisParticleSystem.GetComponent<Renderer>().material = targetMaterial;

            //lower durability

            BreakableObject targetBreakScript = collision.gameObject.GetComponent<BreakableObject>();
            targetBreakScript.materialStrength -= createdForce * 0.7f;

            if (targetBreakScript.materialStrength > 0)
            {
                breakSound.clip = breakClips[0];
                breakSound.Play();
            }

            thisParticleSystem.Play();
        }
        
    }
}
