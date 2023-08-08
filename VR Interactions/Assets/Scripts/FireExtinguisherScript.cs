using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
public class FireExtinguisherScript : MonoBehaviour
{
    private XRBaseInteractable interactable;
    private Renderer cubeRenderer;
    [SerializeField] private ParticleSystem spray;
    [SerializeField] private AudioSource spraySound;

    private bool held = false;
    private bool spraying = false;

    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<XRBaseInteractable>();
        cubeRenderer = GetComponent<Renderer>();
    }
    private void Update()
    {
        if (spraying)
        {
            print("SPRAY");
            //spray.Play();
        }
        else
        {
            //spray.Stop();
        }
    }
    public void InteractableActivate(ActivateEventArgs args)
    {
        spraying = true;
        spray.Play();
        spraySound.Play();
    }

    public void InteractableDeactivate(DeactivateEventArgs args)
    {
        spraying = false;
        spray.Stop();
        spraySound.Stop();
    }
    public void InteractableHeld(SelectEnterEventArgs args)
    {
        held = true;
    }

    public void InteractableUnheld(SelectExitEventArgs args)
    {
        held = false;
    }
}
