using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollMovement : MonoBehaviour
{
    /// <summary>
    /// if initial movement, animated and lerping 
    /// if its dragged, it becomes ragdoll
    /// if not dragged, using animation - may add pointing and move feature 
    /// </summary>
    private bool isInitialMove, isDragged;
    [SerializeField] private Rigidbody ragdoll;
    private Vector3[] masterBedLerpPos;
    private float lerpDuration;
    void Start()
    {
        isInitialMove = true;
        float x1 = 5f;
        float x2 = 20f;
        float z1 = -20f;
        float z2 = -35f;
        float y = 6f;
        masterBedLerpPos = new Vector3[] { new Vector3(x1, y, z1), new Vector3(x2, y, z1), new Vector3(x2, y, z2), new Vector3(x1, y, z2) };

    }

    // Update is called once per frame
    void Update()
    {
        if (isInitialMove)
        {
            // lerp between points
            // master bedroom X[5,20] Y[-20,-35]
            for (int i = 0; i < 3; i++)
            {
                Debug.Log(i);
                StartCoroutine(StartLerp(masterBedLerpPos[i], masterBedLerpPos[i + 1]));
            }
        }
        else if (isDragged)
        {
            // become ragdoll, animation ends/disabled
        }
        else if (!isDragged)
        {
            // disable ragdoll, animation abled
        }
    }

    IEnumerator StartLerp(Vector3 start, Vector3 end)
    {
        float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
            float t = timeElapsed / lerpDuration;
            ragdoll.transform.position = Vector2.Lerp(start, end, t);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        ragdoll.transform.position = end;

    }
}


// ray cast to a random angle, get the hit distance
// depends on how long the raycast is that collide on an object
// if short, ray cast again 
// the hit distance
//RaycastHit hit;
//Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit);