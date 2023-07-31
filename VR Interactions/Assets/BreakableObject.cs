using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BreakableObject : MonoBehaviour
{
    public float materialStrength = 100f;
    private float experiencedForce;

    public ParticleSystem explosion;
    BoxCollider collider;
    MeshRenderer render;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        collider = this.GetComponent<BoxCollider>();
        render = this.GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        experiencedForce = Mathf.Abs(rb.velocity.magnitude * 2 + rb.angularVelocity.magnitude);

        //print(experiencedForce);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody collisionRb = collision.gameObject.GetComponent<Rigidbody>();
        if (collisionRb != null)
        {
            float incomingForce = Mathf.Abs(collisionRb.velocity.magnitude * 2 + collisionRb.angularVelocity.magnitude);

            if (collision.gameObject.CompareTag("BreakTool"))
            {
                print(experiencedForce + incomingForce);

                if (experiencedForce + incomingForce > materialStrength)
                {
                    print("Break");
                    StartCoroutine(Explode());
                }
            }
        }
        else
        {

            if (experiencedForce > materialStrength)
            {
                print("Break");
                StartCoroutine(Explode());
            }
        }
    }
    IEnumerator Explode()
    {
        print("Destroying");

        rb.constraints = RigidbodyConstraints.FreezeAll;
        render.enabled = false;
        collider.enabled = false;

        explosion.Play();

        yield return new WaitForSeconds(6f);

        Destroy(gameObject);
    }
}
