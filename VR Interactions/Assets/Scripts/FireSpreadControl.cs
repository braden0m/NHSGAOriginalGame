using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FireSpreadControl : MonoBehaviour
{
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private GameObject smokePrefab;
    public List<GameObject> allFire;
    public List<GameObject> activeFire;
    public List<GameObject> allSmoke;

    public int fireDirectionResolution;
    public int maxSurroundingFire;
    public float fireSpreadDistance;
    public float fireSpreadTime;

    public bool smokeSpread;
    public float smokeSpreadDistance;
    public float smokeSpreadTime;

    private float fireSpreadCooldown;
    private float smokeSpreadCooldown;

    private Coroutine fireSpreadCoroutine;
    private Coroutine smokeSpreadCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        allFire = GameObject.FindGameObjectsWithTag("Fire").ToList<GameObject>();
        activeFire = GameObject.FindGameObjectsWithTag("Fire").ToList<GameObject>();

        fireSpreadCooldown = fireSpreadTime;
        smokeSpreadCooldown = smokeSpreadTime;
    }

    // Update is called once per frame
    void Update()
    {
        fireSpreadCooldown -= Time.deltaTime;
        smokeSpreadCooldown -= Time.deltaTime;



        if (fireSpreadCooldown < 0 && fireSpreadCoroutine == null)
        {
            fireSpreadCooldown = fireSpreadTime;

            fireSpreadCoroutine = StartCoroutine(ActiveFireSpread());
        }

        if (smokeSpread && smokeSpreadCooldown < 0 && smokeSpreadCoroutine == null)
        {
            smokeSpreadCooldown = smokeSpreadTime;

            smokeSpreadCoroutine = StartCoroutine(SmokeSpread());
        }
        
    }

    bool CanSpread(GameObject fireSeed)
    {
        int nearbyFire = 0;

        foreach (GameObject nextFire in allFire)
        {
            if ((nextFire.transform.position - transform.position).magnitude < fireSpreadDistance && nextFire != gameObject)
            {
                nearbyFire += 1;
            }

            if (nearbyFire > maxSurroundingFire)
            {
                return false;
            }
        }

        return true;
    }

    void SingularFireSpread(GameObject fireSeed)  
    {
        if (CanSpread(fireSeed))
        {

            for (int i = 0; i < fireDirectionResolution; i++)
            {
                RaycastHit nextFireRay;

                Vector3 rayDirection = Quaternion.Euler(0, (360 / fireDirectionResolution * i), 0) * transform.forward;

                Physics.Raycast(fireSeed.transform.position, rayDirection, out nextFireRay, fireSpreadDistance);

                if (nextFireRay.collider == null)
                {
                    GameObject newFire = Instantiate(firePrefab);
                    newFire.name = "FireUnit";
                    //newFire.GetComponent<FireSpread>().initialPosition = transform.position;
                    //newFire.GetComponent<FireSpread>().isSeed = false;

                    newFire.transform.position = fireSeed.transform.position + rayDirection * fireSpreadDistance;
                    //newFire.GetComponent<FireSpread>().canSpread = true;

                    activeFire.Add(newFire);
                    allFire.Add(newFire);
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

        }

        activeFire.Remove(fireSeed);
    }

    void SingularSmokeSpread(GameObject fireSeed)
    {
        int nearbySmoke = 0;
        bool canSpreadSmoke = true;

        foreach (GameObject nextSmoke in allSmoke)
        {
            if ((nextSmoke.transform.position - fireSeed.transform.position).magnitude < smokeSpreadDistance && nextSmoke != gameObject)
            {
                canSpreadSmoke = false;
                break;
            }
        }

        if (canSpreadSmoke)
        {
            GameObject newSmoke = Instantiate(smokePrefab);
            newSmoke.name = "SmokeUnit";
            newSmoke.transform.position = fireSeed.transform.position + Vector3.up * 0.4f;

            allSmoke.Add(newSmoke);
        }
    }

    IEnumerator ActiveFireSpread()
    {
        int currentFire = allFire.Count();
        int i = 0;

        //List<GameObject> nextWaveFire = new List<GameObject>(allFire);

        while (i < allFire.Count())
        {
            SingularFireSpread(allFire[i]);
            yield return null;
        }

        /*
        for (int i = currentFire - 1; i >= 0; i--)
        {
            SingularFireSpread(nextWaveFire[i]);
            yield return null;

            if (allFire.Count() - i == 0)
            {
                break;
            }
        }
        */

        fireSpreadCoroutine = null;
    }

    IEnumerator SmokeSpread()
    {
        int currentFire = allFire.Count();

        List<GameObject> nextWaveFire = new List<GameObject>(allFire);

        for (int i = currentFire - 1; i >= 0; i--)
        {
            SingularSmokeSpread(nextWaveFire[i]);
            yield return null;
        }

        smokeSpreadCoroutine = null;
    }
}
