using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using UnityEditor;
using System.Linq;

public class NewRagdollState : MonoBehaviour
{
    [SerializeField] private GameObject pelvis;
    [SerializeField] private GameObject armature;

    private Animator animator;
    private Collider mainCollider;
    private Rigidbody mainRigidbody;
    [SerializeField] private List<Rigidbody> ragdollParts;
    [SerializeField] private List<Collider> ragdollColliders;

    public List<Transform> waypoints;
    private int currentWaypointIndex;

    private bool isGrabbed = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        mainCollider = GetComponent<Collider>();
        mainRigidbody = GetComponent<Rigidbody>();

        ragdollColliders = GetComponentsInChildren<Collider>().ToList<Collider>();
        ragdollParts = GetComponentsInChildren<Rigidbody>().ToList<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDifference = waypoints[currentWaypointIndex].transform.position - transform.position;

        mainRigidbody.velocity = targetDifference.normalized * 5;

        if (!isGrabbed)
        {
            transform.rotation = Quaternion.Euler(0, Mathf.Atan2(targetDifference.x, targetDifference.z) * Mathf.Rad2Deg, 0);

            
        }

        if (((Vector3) transform.position - (Vector3) waypoints[currentWaypointIndex].transform.position).magnitude < 1) {
            currentWaypointIndex = NextWaypoint();
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
            rb.isKinematic = true;
        }
        mainCollider.enabled = true;
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
        mainCollider.enabled = false;
    }

    public void InteractableHeld(SelectEnterEventArgs args)
    {
        isGrabbed = true;
    }

    public void InteractableUnheld(SelectExitEventArgs args)
    {
        isGrabbed = false;
    }
}
