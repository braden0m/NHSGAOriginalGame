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
    // Start is called before the first frame update
    void Start()
    {
        spray = GetComponent<ParticleSystem>();
        interactable = GetComponent<XRBaseInteractable>();
        cubeRenderer = GetComponent<Renderer>();
        originalColor = cubeRenderer.material.color;
    }
    public void InteractableActivate(ActivateEventArgs args)
    {
        spray.Play();
    }

    public void InteractableDeactivate(DeactivateEventArgs args)
    {
        spray.Stop();
    }

    public void InteractableHover(HoverEnterEventArgs args)
    {
        cubeRenderer.material.color = newColor;
    }
    public void InteractableUnhover(HoverExitEventArgs args)
    {
        cubeRenderer.material.color = originalColor;
    }
}
