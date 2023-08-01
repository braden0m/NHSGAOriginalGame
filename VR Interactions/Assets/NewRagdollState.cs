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
    [SerializeField] private bool isMoving = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        mainCollider = GetComponent<Collider>();
        mainRigidbody = GetComponent<Rigidbody>();

        ragdollColliders = GetComponentsInChildren<Collider>().ToList<Collider>();
        ragdollParts = GetComponentsInChildren<Rigidbody>().ToList<Rigidbody>();

        RagdollModeOff();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Running", isMoving);

        if (!isGrabbed)
        {
            //transform.rotation = Quaternion.Euler(0, Mathf.Atan2(targetDifference.x, targetDifference.z) * Mathf.Rad2Deg, 0);
            Vector3 targetPositionalDifference = waypoints[currentWaypointIndex].transform.position - transform.position;
            //transform.position += targetPositionalDifference.normalized * 3 * Time.deltaTime;

            transform.position = Vector3.SmoothDamp(transform.position, waypoints[currentWaypointIndex].transform.position, ref currentVelocity, targetPositionalDifference.magnitude / moveSpeed);

            //mainRigidbody.velocity = targetPositionalDifference.normalized * 5;

            //Vector3 targetRotation = Quaternion.LookRotation(targetPositionalDifference, Vector3.up).eulerAngles;

            transform.LookAt(waypoints[currentWaypointIndex].transform);
        }

        if (((Vector3)transform.position - (Vector3)waypoints[currentWaypointIndex].transform.position).magnitude < 1)
        {
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

        RagdollModeOn();
    }

    public void InteractableUnheld(SelectExitEventArgs args)
    {
        isGrabbed = false;

        RagdollModeOff();
    }
}