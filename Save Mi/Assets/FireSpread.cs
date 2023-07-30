using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpread : MonoBehaviour
{
    public GameObject fire;
    public bool canSpread;
    public bool isSeed;
    public Material burntMaterial;

    public Vector3 initialPosition;
    public Vector3 finalPosition;
    
    float lifetime = 7f;

    float randomSpreadDistance = 2;
    float randomSpreadSpeed = 1;

    ParticleSystem flame;
    SphereCollider collider;

    // Start is called before the first frame update
    void Start()
    {
        flame = GetComponent<ParticleSystem>();
        collider = GetComponent<SphereCollider>();

        float randomSpreadDistance = Random.Range(1.5f, 2f);
        float randomSpreadSpeed = Random.Range(0.5f, 1f);

        //print(randomSpreadDistance);

        int nearbyFire = 0;

        GameObject[] allFires = GameObject.FindGameObjectsWithTag("Fire");

        foreach (GameObject nextFire in allFires)
        {
            if ((nextFire.transform.position - transform.position).magnitude < randomSpreadDistance && nextFire != gameObject)
            {
                nearbyFire += 1;
            }
        }

        if (nearbyFire > 5)
        {
            canSpread = false;
        }

        finalPosition = transform.position;

        StartCoroutine(InitialAction());
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;

        if (lifetime < 0)
        {
            StartCoroutine(BurntOut());
        }
    }
    void SpreadFire()
    {
        int resolution = Mathf.RoundToInt(Random.Range(4, 8));

        for (int i = 0; i < resolution; i++)
        {
            RaycastHit nextFireRay;

            Vector3 rayDirection = Quaternion.Euler(0, (360/resolution * i), 0) * transform.up;

            Physics.Raycast(transform.position, rayDirection, out nextFireRay, randomSpreadDistance);

            if (nextFireRay.collider == null)
            {
                GameObject newFire = Instantiate(fire);
                newFire.name = "FireUnit";
                newFire.GetComponent<FireSpread>().initialPosition = transform.position;
                newFire.GetComponent<FireSpread>().isSeed = false;

                newFire.transform.position = transform.position + rayDirection * randomSpreadDistance;
                newFire.GetComponent<FireSpread>().canSpread = true;
            }

            /*
            GameObject newFire = Instantiate(fire);
            newFire.GetComponent<FireSpread>().initialPosition = transform.position;
            newFire.GetComponent<FireSpread>().isSeed = false;

            if (nextFireRay.collider != null)
            {
                newFire.transform.position = nextFireRay.point;
                newFire.GetComponent<FireSpread>().canSpread = false;
            }
            else
            {
                newFire.transform.position = transform.position + rayDirection * randomSpreadDistance;
                newFire.GetComponent<FireSpread>().canSpread = true;
            }

            */
        }

        canSpread = false;
    }

    IEnumerator InitialAction()
    {
        if (!isSeed)
        {
            float timeScale = 0;

            while (timeScale < randomSpreadSpeed)
            {
                print(timeScale);

                timeScale += Time.deltaTime;
                transform.position = Vector3.Lerp(initialPosition, finalPosition, timeScale / randomSpreadSpeed);

                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        else
        {
            yield return new WaitForSeconds(randomSpreadSpeed);
        }

        if (canSpread)
        {
            SpreadFire();
        }
    }

    IEnumerator BurntOut()
    {
        flame.enableEmission = false;
        collider.enabled = false;

        yield return new WaitForSeconds(3);

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Burnable"))
        {
            other.gameObject.GetComponent<MeshRenderer>().material = burntMaterial;
        }
    }
}
