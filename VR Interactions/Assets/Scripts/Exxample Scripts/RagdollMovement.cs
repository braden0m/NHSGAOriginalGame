using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using UnityEditor;
using System.Linq;
using UnityEngine.AI; //important

public class RagdollMovement : MonoBehaviour
{
    /// <summary>
    /// if initial movement, animated and lerping 
    /// if its dragged, it becomes ragdoll
    /// if not dragged, using animation - may add pointing and move feature 
    /// </summary>
    /*
    public NavMeshAgent agent;
    private bool isInitialMove, canInteract, isGrabbed;
    [SerializeField] private Rigidbody mainRb;
    [SerializeField] private GameObject thisRagdoll;
    [SerializeField] private Animator thisAnim;
    [SerializeField] private Collider mainCollider;
    [SerializeField] private List<Rigidbody> ragdollParts;
    [SerializeField] private List<Collider> ragdollColliders;

    public Collider spineCollider;

    //public List<Transform> lerpLocations;
    private float lerpDuration, ragAge;
    //private int count, countLerps;
    public Transform startLocation;
    //private Coroutine currentCoroutine;

    //public GameObject armature;

    float testTimer = 1f;
    public bool ragdollingTest = false;

    // ragdoll movement
    private XRBaseInteractable interactable;

    */
    private bool canInteract;
    private float ragAge;
    public Material burnMaterial;
    [SerializeField] Color originalColor, transparentColor;
    [SerializeField] Color newColor;
    [SerializeField] private SkinnedMeshRenderer ragDoll;

    void Start()
    {
        //isInitialMove = true;
        //GetRagdollComponents();
        //RagdollModeOff();
        //lerpDuration = 3f;
        // store the time it can stay in the fire 
        ragAge = 0f;
        //count = 0;
        //countLerps = lerpLocations.Count;

        canInteract = true;
        //agent.SetDestination(startLocation.position);
        //thisAnim.SetBool("Running", true);
    }

    // Update is called once per frame
    //void Update()
    //{
    //agent.SetDestination(startLocation.position
    //if (isGrabbed)
    //{
    //    armature.transform.position = spineCollider.transform.position;
    //}

    //if (isInitialMove && !isGrabbed)
    //{
    //    thisAnim.SetBool("isRun", true);
    //    // lerp between points
    //    if (currentCoroutine == null)
    //    {
    //        //Continue from current position instead of last waypoint, to remove teleporting
    //        //currentCoroutine = StartCoroutine(StartLerp(lerpLocations[count% countLerps].position, lerpLocations[(count+1)% countLerps].position));
    //        currentCoroutine = StartCoroutine(StartLerp(mainRb.transform.position, lerpLocations[(count + 1) % countLerps].position));
    //    }
    //    //isInitialMove = false;
    //    //print(isInitialMove);
    //}

    /*
    if (agent.enabled)
    {
        if (agent.remainingDistance <= agent.stoppingDistance || agent.velocity.sqrMagnitude <= 0.03f)
        {
            thisAnim.SetBool("Running", false);
        }
    }
     */

    //testTimer -= Time.deltaTime;

    //if (testTimer < 0)
    //{
    //    testTimer = 1f;

    //    if (ragdollingTest)
    //    {
    //        ragdollingTest = false;
    //        //RagdollModeOff();
    //    } else
    //    {
    //        ragdollingTest = true;
    //        //RagdollModeOn();
    //    }
    //}


    /*
    private void GetRagdollComponents()
    {
        ragdollColliders = GetComponentsInChildren<Collider>().ToList<Collider>();
        ragdollParts = GetComponentsInChildren<Rigidbody>().ToList<Rigidbody>();

        ragdollColliders.Remove(spineCollider);
    }

    private void RagdollModeOff()
    {
        thisAnim.enabled = true;
        thisAnim.SetBool("Running", true);
        foreach (Collider ragCol in ragdollColliders)
        {
            ragCol.enabled = false;
        }
        foreach (Rigidbody rb in ragdollParts)
        {
            rb.isKinematic = true;
        }
        mainCollider.enabled = true;
        //mainRb.isKinematic = false;

        //thisAnim.SetBool("isRun", true);
        agent.enabled = true;
        Debug.Log("ragdoll mode off");
    }

    private void RagdollModeOn()
    {
        agent.enabled = false;
        thisAnim.enabled = false;
        foreach (Collider ragCol in ragdollColliders)
        {
            ragCol.enabled = true;
        }
        foreach (Rigidbody rb in ragdollParts)
        {
            rb.isKinematic = false;
        }
        mainCollider.enabled = false;
        mainRb.isKinematic = true;
    }

    */

    // vr actions
    //public void InteractableHeld(SelectEnterEventArgs args)
    //{
    //    if (canInteract)
    //    {
    //        //isGrabbed = true;

    //        //Stop lerp
    //        //if (currentCoroutine != null)
    //        //{
    //        //    StopCoroutine(currentCoroutine);
    //        //    currentCoroutine = null;
    //        //}

    //        //isInitialMove = false;
    //        //RagdollModeOn();

    //    }
    //}

    //public void InteractableUnheld(SelectExitEventArgs args)
    //{
    //    if (canInteract)
    //    {
    //        isGrabbed = false;
    //        isInitialMove = false;
    //        RagdollModeOff();

    //    }
    //}

    private void OnTriggerStay(Collider other)
    {
        // ! add tag
        if (ragAge > 10f)
        {
            Debug.Log("DIE");
            // want this to be only called once.
            if (canInteract)
            {
                canInteract = false;
                ragDoll.materials[0].color = transparentColor;
                ragDoll.materials[1].color = transparentColor;
                GameManager.instance.RagdollLife();
                return;
            }
            
            //StartCoroutine(Die());
        }
        Debug.Log("hha");
        if (other.gameObject.tag == "Fire")
        {
            //RagdollModeOn();
            ragAge += Time.deltaTime;
            Debug.Log(ragAge);
            Color lerpedColor = Color.Lerp(originalColor, newColor, ragAge/10f);
            ragDoll.materials[0].color = lerpedColor;
            //burnMaterial.color = lerpedColor;
            // fade out by falling into the ground
        }
    }

    IEnumerator Die()
    {
        //// play dying anim/particle system, tell the game manager
        //die.Play();
        // ! game manager
        // can no longer do actions on them, ! maybe can put flowers lol
        canInteract = false;
        yield return new WaitForSeconds(10f);
        //Destroy(thisRagdoll);
    }

}


// ray cast to a random angle, get the hit distance
// depends on how long the raycast is that collide on an object
// if short, ray cast again 
// the hit distance
//RaycastHit hit;
//Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit);


/*
    IEnumerator StartLerp(Vector3 start, Vector3 end)
    {
        float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
            float t = timeElapsed / lerpDuration;
            mainRb.transform.position = Vector3.Lerp(start, end, t);
            timeElapsed += Time.deltaTime;

            yield return new WaitForSeconds(Time.deltaTime);

            //print(timeElapsed);
        }
        mainRb.transform.position = end;
        thisRagdoll.transform.Rotate(0.0f, 90.0f, 0.0f);
        count++;

        Debug.Log("coroutine");
        currentCoroutine = null;


    }
    */