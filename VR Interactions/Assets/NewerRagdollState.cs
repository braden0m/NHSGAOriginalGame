using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using UnityEditor;
using System.Linq;
public class NewerRagdollState : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    [SerializeField] private GameObject pelvis;
    [SerializeField] private GameObject armature;

    private Animator animator;
    private Collider mainCollider;
    private Rigidbody mainRigidbody;
    [SerializeField] private List<Rigidbody> ragdollParts;
    [SerializeField] private List<Collider> ragdollColliders;

    private int currentWaypointIndex;

    private Vector3 currentVelocity;
    private bool isGrabbed = false;
    private Vector3 lastGrabbedLocation;
    private Coroutine dazedCoroutine;
    [SerializeField] private bool isMoving = false;

    // Life Manage
    private float ragAge;
    public bool canInteract = true;
    public bool isRagdoll;
    private bool previousIsRagdoll;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        mainCollider = GetComponent<Collider>();
        mainRigidbody = GetComponent<Rigidbody>();

        ragdollColliders = GetComponentsInChildren<Collider>().ToList<Collider>();
        ragdollParts = GetComponentsInChildren<Rigidbody>().ToList<Rigidbody>();

        ragdollParts.Remove(mainRigidbody);
        ragdollColliders.Remove(mainCollider);
        ragdollColliders.Remove(pelvis.GetComponent<Collider>());

        RagdollModeOff();
        isGrabbed = false;
        canInteract = true;

        previousIsRagdoll = isRagdoll;

        //mainCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Running", isMoving);

        if (isRagdoll != previousIsRagdoll)
        {
            if (isRagdoll)
            {
                RagdollModeOn();
            }
            else
            {
                RagdollModeOff();
            }
        }
        previousIsRagdoll = isRagdoll;
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

    private void RagdollModeOff()
    {
        mainRigidbody.constraints = RigidbodyConstraints.None;
        mainCollider.enabled = true;
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
        mainRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        mainCollider.enabled = false;
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
