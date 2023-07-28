using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

//This script is just an example of how to get the velocity of the hand
// This script is attached to the HandModel prefab
public class HandVelocity : MonoBehaviour
{
    [SerializeField] bool isLeftHand;
    
    [SerializeField] private InputActionReference leftVelocity;
    [SerializeField] private InputActionReference rightVelocity;
    [SerializeField] TextMeshPro text;
    private InputAction action;
    
    // Start is called before the first frame update
    void Start()
    {
        switch(isLeftHand)
        {
            case true:
                action = leftVelocity.action;
                break;
            case false:
                action = rightVelocity.action;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //This is just an example of how to get the velocity of the hand
        text.text = action.ReadValue<Vector3>().magnitude.ToString("F2");
    }
}
