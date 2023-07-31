using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
public class FireExtinguisherScript : MonoBehaviour
{
    private XRBaseInteractable interactable;
    private Renderer cubeRenderer;
    private Color originalColor;
    [SerializeField] private ParticleSystem spray;
    [SerializeField] private Color newColor;

    private bool held = false;
    private bool spraying = false;

    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<XRBaseInteractable>();
        cubeRenderer = GetComponent<Renderer>();
        originalColor = cubeRenderer.material.color;
    }
    private void Update()
    {
        if (spraying && held)
        {
            spray.Play();
        }
        else
        {
            spray.Stop();
        }
    }
    public void InteractableActivate(ActivateEventArgs args)
    {
        spraying = true;
    }
    public void InteractableHeld(SelectEnterEventArgs args)
    {
        held = true;
        cubeRenderer.material.color = originalColor;
    }

    public void InteractableUnheld(SelectExitEventArgs args)
    {
        held = false;
        cubeRenderer.material.color = originalColor;
    }
    public void InteractableDeactivate(DeactivateEventArgs args)
    {
        spraying = false;
        spray.Stop();
    }
    public void InteractableHover(HoverEnterEventArgs args)
    {
        if (!held)
        {
            cubeRenderer.material.color = newColor;
        }
    }
    public void InteractableUnhover(HoverExitEventArgs args)
    {
        cubeRenderer.material.color = originalColor;
    }
}
