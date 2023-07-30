using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //important

public class AIMovement : MonoBehaviour //don't forget to change the script name if you haven't
{

    public Transform movementDest;
    public GameObject currentRoom;
    [SerializeField] private float detectDistance = 0.3f;

    [SerializeField] private float speed = 5f;
    [SerializeField] private ConfigurableJoint hipJoint;
    [SerializeField] private Rigidbody hip;

    [SerializeField] private Animator targetAnimator;

    private bool walk = false;

    void Update()
    {
        
        //if (movementDest.position.x - transform.position.x < detectDistance && movementDest.position.z - transform.position.z < detectDistance)
        //{
            
        //    movementDest.position = new Vector3();
        //}

        Vector3 direction = (movementDest.position - transform.position).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

            this.hipJoint.targetRotation = Quaternion.Euler(0f, targetAngle, 0f);

            this.hip.AddForce(direction * this.speed);

            this.walk = true;
        }
        else
        {
            // have a chance of staying for seconds
            if (Random.Range(0, 1f) > 0.6)
            {
                this.walk = false;
            }
            else
            {
                // if too close, set a new movement destination
                movementDest.position = new Vector3();
            } 
        }

        this.targetAnimator.SetBool("Walk", this.walk);
    }


}

/*
 if (agent.remainingDistance <= agent.stoppingDistance) //done with path
        {
            Vector3 point;
            if (RandomPoint(agent.gameObject.transform.position, range, out point)) //pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                agent.SetDestination(point);
            }
        }
    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) //https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
    */