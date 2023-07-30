using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //important

//if you use this code you are contractually obligated to like the YT video
public class RagdollRandomMovement : MonoBehaviour //don't forget to change the script name if you haven't
{
    public NavMeshAgent agent;
    public float range; //radius of sphere

    public Transform centrePoint; //centre of the area the agent wants to move around in -- 7f for testing room

    // the ragdoll turn its hip
    [SerializeField] private float speed = 5f;
    [SerializeField] private ConfigurableJoint hipJoint;
    [SerializeField] private GameObject hip;
    //[SerializeField] private GameObject ragdoll;
    //[SerializeField] private Animator targetAnimator;

    private bool walk = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance) //done with path
        {
            this.walk = false;
            //this.targetAnimator.SetBool("Walk", this.walk);
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point)) //pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //gizmos
                agent.SetDestination(point);

                Vector3 direction = new Vector3(point.x-transform.position.x, 0f, point.z - transform.position.z).normalized;
                
                if (direction.magnitude != 0f)
                {
                    //float targetAngle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;// 

                    //hipJoint.targetRotation = Quaternion.Euler(0f, targetAngle, 0f);

                    //hip.transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

                    //hipJoint.targetRotation = Quaternion.Euler(0f, 90f, 0f); 
                    //Debug.Log(hipJoint.targetRotation);
                    //this.hip.AddForce(direction * this.speed);
                    //ragdoll.transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

                    this.walk = true;
                }
                else
                {
                    
                    this.walk = false;
                }

                //this.targetAnimator.SetBool("Walk", this.walk);
            }

            //Vector3 direction = (point-transform.position).normalized;
            //Debug.Log(direction);
            //Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, 1f*Time.deltaTime, 0.0f);
            ////hip.transform.rotation = Quaternion.LookRotation(newDir);

            //// move its hip
            //float targetAngle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
            //this.hipJoint.targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            //hip.AddForce(direction * this.speed);
            //Debug.Log(hip.transform.rotation);
            //this.walk = true;

        


        }
        //this.targetAnimator.SetBool("Walk", this.walk);
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }


}