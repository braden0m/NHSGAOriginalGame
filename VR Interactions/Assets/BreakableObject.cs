using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BreakableObject : MonoBehaviour
{
    public float materialStrength = 5f;
    private float experiencedForce;
    private List<GameObject> mainInteactableScriptList;

    public ParticleSystem explosion;
    public AudioSource explosionSound;
    Collider collider;
    MeshRenderer render;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        collider = this.GetComponent<Collider>();
        render = this.GetComponent<MeshRenderer>();

        mainInteactableScriptList = GameObject.Find("InteractableController").GetComponent<InteractableMainScript>().allInteractables;
    }

    // Update is called once per frame
    void Update()
    {
        if (rb != null)
        {
            experiencedForce = Mathf.Abs(rb.velocity.magnitude * 2 + rb.angularVelocity.magnitude);
        }
        else
        {
            rb = GetComponent<Rigidbody>();
        }

        //print(experiencedForce);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BreakTool") && collision.gameObject.GetComponent<AxeScript>() != null)
        {
            float createdForce = collision.gameObject.GetComponent<AxeScript>().createdForce;
            print(experiencedForce + createdForce);

            if (experiencedForce + createdForce > materialStrength)
            {
                print("Break");
                StartCoroutine(Explode());
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

        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        render.enabled = false;
        collider.enabled = false;

        Debug.Log("explosion played");

        explosion = gameObject.GetComponentInChildren<ParticleSystem>();
        if (explosion != null)
        {
            explosion.Play();
        }

        if (explosionSound != null)
        {
            explosionSound.Play();
        }
        yield return new WaitForSeconds(6f);

        if (mainInteactableScriptList.Contains(this.gameObject))
        {
            mainInteactableScriptList.Remove(this.gameObject);
        }

        Destroy(gameObject);
    }
}
