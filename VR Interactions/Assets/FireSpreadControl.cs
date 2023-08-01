using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FireSpreadControl : MonoBehaviour
{
    public GameObject firePrefab;
    public List<GameObject> allFire;
    public List<GameObject> activeFire;

    public int fireDirectionResolution = 8;
    public int maxSurroundingFire = 5;
    public float spreadDistance = 2f;
    public float spreadTime = 2f;

    private float spreadCooldown;

    private Coroutine fireSpreadCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        allFire = GameObject.FindGameObjectsWithTag("Fire").ToList<GameObject>();
        activeFire = GameObject.FindGameObjectsWithTag("Fire").ToList<GameObject>();

        spreadCooldown = spreadTime;
    }

    // Update is called once per frame
    void Update()
    {
        spreadCooldown -= Time.deltaTime;

        
        if (spreadCooldown < 0 && fireSpreadCoroutine == null)
        {
            spreadCooldown = 1f;

            fireSpreadCoroutine = StartCoroutine(ActiveFireSpread());
        }
        
    }

    bool CanSpread(GameObject fireSeed)
    {
        int nearbyFire = 0;

        foreach (GameObject nextFire in allFire)
        {
            if ((nextFire.transform.position - transform.position).magnitude < spreadDistance && nextFire != gameObject)
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

                Physics.Raycast(fireSeed.transform.position, rayDirection, out nextFireRay, spreadDistance);

                if (nextFireRay.collider == null)
                {
                    GameObject newFire = Instantiate(firePrefab);
                    newFire.name = "FireUnit";
                    //newFire.GetComponent<FireSpread>().initialPosition = transform.position;
                    //newFire.GetComponent<FireSpread>().isSeed = false;

                    newFire.transform.position = fireSeed.transform.position + rayDirection * spreadDistance;
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

    IEnumerator ActiveFireSpread()
    {
        int currentFire = activeFire.Count();

        for (int i = currentFire - 1; i >= 0; i--)
        {
            SingularFireSpread(activeFire[i]);
            yield return null;

            if (activeFire.Count() - i == 0)
            {
                break;
            }
        }

        fireSpreadCoroutine = null;
    }
}
