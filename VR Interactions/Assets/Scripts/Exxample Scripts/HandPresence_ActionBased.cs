using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;


public class HandPresence_ActionBased : MonoBehaviour
{
    private ActionBasedController controller;
    private Animator animator;

    public InputActionReference primaryButton;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponentInParent<ActionBasedController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Grip", controller.selectAction.action.ReadValue<float>());
        animator.SetFloat("Trigger", controller.activateAction.action.ReadValue<float>());

        if (primaryButton && primaryButton.action.WasPressedThisFrame())
        {
            Debug.Log("Right Hand Primary Button is pressed.");
        }
    }
}
