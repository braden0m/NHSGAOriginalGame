using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HandPresence_DeviceBased : MonoBehaviour
{    
    private InputDevice controller;
    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        // --- Get Devices ---
        List<InputDevice> devices = new List<InputDevice>();

        // Get all the devices and print out
        InputDevices.GetDevices(devices);
        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }

        // Get the right controller
        InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);
        if (devices.Count > 0)
        {
            controller = devices[0];
        }
        else
        {
            Debug.LogWarning("Could not find the right controller");
        }


        // --- Get rightHand animator---
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
        if (controller.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
            animator.SetFloat("Grip", gripValue);
       
        
        if (controller.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
            animator.SetFloat("Trigger", triggerValue);

    }
    
}
