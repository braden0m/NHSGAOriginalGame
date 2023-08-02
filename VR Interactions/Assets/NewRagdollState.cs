using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using UnityEditor;
using System.Linq;

public class NewRagdollState : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    [SerializeField] private GameObject pelvis;
    [SerializeField] private GameObject armature;

    private Animator animator;
    private Collider mainCollider;
    private Rigidbody mainRigidbody;
    [SerializeField] private List<Rigidbody> ragdollParts;
    [SerializeField] private List<Collider> ragdollColliders;

    public List<Transform> waypoints;
    private int currentWaypointIndex;

    private Vector3 currentVelocity;
    private bool isGrabbed = false;
    private Vector3 lastGrabbedLocation;
    private Coroutine dazedCoroutine;
    [SerializeField] private bool isMoving = true;

    // Life Manage
    private float ragAge;
    public bool canInteract = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        mainCollider = armature.GetComponent<Collider>();
        mainRigidbody = armature.GetComponent<Rigidbody>();

        ragdollColliders = GetComponentsInChildren<Collider>().ToList<Collider>();
        ragdollParts = GetComponentsInChildren<Rigidbody>().ToList<Rigidbody>();

        ragdollParts.Remove(mainRigidbody);
        ragdollColliders.Remove(mainCollider);
        ragdollColliders.Remove(pelvis.GetComponent<Collider>());

        RagdollModeOff();
        isGrabbed = false;
        canInteract = true;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(lastGrabbedLocation);

        animator.SetBool("Running", isMoving);

        if (!isGrabbed && isMoving)
        //if (true)
        {
            //transform.rotation = Quaternion.Euler(0, Mathf.Atan2(targetDifference.x, targetDifference.z) * Mathf.Rad2Deg, 0);
            Vector3 targetPositionalDifference = waypoints[currentWaypointIndex].transform.position - armature.transform.position;
            //transform.position += targetPositionalDifference.normalized * 3 * Time.deltaTime;
            //transform.position = Vector3.SmoothDamp(armature.transform.position, waypoints[currentWaypointIndex].transform.position, ref currentVelocity, targetPositionalDifference.magnitude / moveSpeed);
            mainRigidbody.velocity = targetPositionalDifference.normalized * 50 * Time.deltaTime;

            //Vector3 targetRotation = Quaternion.LookRotation(targetPositionalDifference, Vector3.up).eulerAngles;

            Quaternion lookDirection = Quaternion.LookRotation(-targetPositionalDifference, Vector3.up);

            armature.transform.rotation = lookDirection;
            pelvis.transform.rotation = armature.transform.rotation;

            //armature.transform.LookAt(waypoints[currentWaypointIndex].transform.position);
        }

        if (((Vector3)mainRigidbody.transform.position - (Vector3)waypoints[currentWaypointIndex].transform.position).magnitude < 1)
        {
            Debug.Log("Enter");
            currentWaypointIndex = NextWaypoint();
        }
    }

    // Grace
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
    private int NextWaypoint()
    {
        if (currentWaypointIndex + 1 == waypoints.Count())
        {
            return 0;
        }
        return currentWaypointIndex + 1;
    }
    private void RagdollModeOff()
    {
        animator.enabled = true;
        foreach (Collider ragCol in ragdollColliders)
        {
            ragCol.enabled = false;
        }
        foreach (Rigidbody rb in ragdollParts)
        {
            rb.isKinematic = false;
        }
    }

    private void RagdollModeOn()
    {
        animator.enabled = false;
        foreach (Collider ragCol in ragdollColliders)
        {
            ragCol.enabled = true;
        }
        foreach (Rigidbody rb in ragdollParts)
        {
            rb.isKinematic = false;
        }
    }

    public void InteractableHeld(SelectEnterEventArgs args)
    {
        if (canInteract)
        {
            isGrabbed = true;

            RagdollModeOn();

            if (dazedCoroutine != null)
            {
                StopCoroutine(dazedCoroutine);
                dazedCoroutine = null;
            }
        }
        
    }

    public void InteractableUnheld(SelectExitEventArgs args)
    {
        if (canInteract)
        {
            isGrabbed = false;

            if (dazedCoroutine != null)
            {
                StopCoroutine(dazedCoroutine);
                dazedCoroutine = null;
            }

            dazedCoroutine = StartCoroutine(Daze());
        }
    }

    IEnumerator Die()
    {
        //// play dying anim/particle system, tell the game manager
        //die.Play();
        // ! game manager
        // can no longer do actions on them, ! maybe can put flowers lol
        canInteract = false;
        RagdollModeOn();
        GameManager.instance.RagdollLife();
        yield return new WaitForSeconds(10f);
        mainRigidbody.gameObject.SetActive(false);
    }

    IEnumerator Daze()
    {
        //Short stun right after being unheld

        pelvis.transform.position = lastGrabbedLocation;

        yield return new WaitForSeconds(5f);
        RagdollModeOff();
        mainRigidbody.velocity = Vector3.zero;
        lastGrabbedLocation = pelvis.transform.position;
    }
}