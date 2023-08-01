using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using UnityEditor;

public class RagdollMovement : MonoBehaviour
{
    /// <summary>
    /// if initial movement, animated and lerping 
    /// if its dragged, it becomes ragdoll
    /// if not dragged, using animation - may add pointing and move feature 
    /// </summary>
    private bool isInitialMove, canInteract;
    [SerializeField] private Rigidbody mainRb;
    [SerializeField] private GameObject thisRagdoll;
    [SerializeField] private Animator thisAnim;
    [SerializeField] private Collider mainCollider;
    [SerializeField] private Rigidbody[] ragdollParts;
    [SerializeField] private Collider[] ragdollColliders;

    public List<Transform> lerpLocations;
    private float lerpDuration, ragAge;
    private int count, countLerps;
    private Coroutine currentCoroutine;


    // ragdoll movement
    private XRBaseInteractable interactable;
    
    void Start()
    {
        isInitialMove = true;
        GetRagdollComponents();
        RagdollModeOff();
        lerpDuration = 1f;
        // store the time it can stay in the fire 
        ragAge = 0f;
        count = 0;
        countLerps = lerpLocations.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInitialMove)
        {
            thisAnim.SetBool("isRun", true);
            // lerp between points
            if (currentCoroutine == null)
            {
                currentCoroutine = StartCoroutine(StartLerp(lerpLocations[count% countLerps].position, lerpLocations[(count+1)% countLerps].position));
            }
            //isInitialMove = false;
            //print(isInitialMove);
        }
    }

    private void GetRagdollComponents()
    {
        ragdollColliders = GetComponentsInChildren<Collider>();
        ragdollParts = GetComponentsInChildren<Rigidbody>();
    }

    private void RagdollModeOff()
    {
        thisAnim.enabled = true;
        foreach (Collider ragCol in ragdollColliders)
        {
            ragCol.enabled = false;
        }
        foreach (Rigidbody rb in ragdollParts)
        {
            rb.isKinematic = true;
        }
        mainCollider.enabled = true;
        mainRb.isKinematic = false;
    }

    private void RagdollModeOn()
    {
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

    // vr actions
    public void InteractableHeld(SelectEnterEventArgs args)
    {
        if (canInteract)
        {
            isInitialMove = false;
            RagdollModeOn();
        }
    }

    public void InteractableUnheld(SelectExitEventArgs args)
    {
        if (canInteract)
        {
            isInitialMove = false;
            RagdollModeOff();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // ! add tag
        if (other.gameObject.tag == "Fire")
        {
            RagdollModeOn();
            ragAge += Time.deltaTime;
            // fade out by falling into the ground
            if (ragAge > 2f)
            {
                StartCoroutine(Die());
            }
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
        Destroy(thisRagdoll);
    }

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

        currentCoroutine = null;
    }
}


// ray cast to a random angle, get the hit distance
// depends on how long the raycast is that collide on an object
// if short, ray cast again 
// the hit distance
//RaycastHit hit;
//Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit);