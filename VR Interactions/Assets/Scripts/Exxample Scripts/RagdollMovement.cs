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
    void Start()
    {
        isInitialMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInitialMove)
        {

        }

        else if (isDragged)
        {

        }
        else if (!isDragged)
        {

        }
    }
}
