using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
public class FireExtinguisherScript : MonoBehaviour
{
    private XRBaseInteractable interactable;
    private Renderer cubeRenderer;
    [SerializeField] private FireSpreadControl fireControl;
    private Color originalColor;
    [SerializeField] private ParticleSystem spray;
    [SerializeField] private AudioSource spraySound;
    [SerializeField] private GameObject waterDisplay;
    [SerializeField] private RectTransform waterLevelBar;

    [SerializeField] private float waterlevel;
    [SerializeField] private Color hoverColor;

    private bool held = false;
    private bool spraying = false;

    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<XRBaseInteractable>();
        cubeRenderer = GetComponent<Renderer>();
        originalColor = cubeRenderer.material.color;
        spraySound = GetComponent<AudioSource>();
    }
    private void FixedUpdate()
    {
        waterDisplay.SetActive(held);
        waterLevelBar.localPosition = new Vector3(0, (3 - waterlevel) / 6, 0);
        waterLevelBar.localScale = new Vector3(1, waterlevel / 3, 1);


        if (spraying && waterlevel > 0 && held)
        {
            spray.Play();
            spraySound.Play();

            waterlevel -= 0.01f;

            RaycastHit rayhit;
            Physics.Raycast(spray.transform.position, spray.transform.forward, out rayhit, 4);

            if (rayhit.collider != null)
            {
                if (rayhit.collider.gameObject.CompareTag("Fire"))
                {
                    fireControl.allFire.Remove(rayhit.collider.gameObject);
                    Destroy(rayhit.collider.gameObject);
                }
            }

        }
        else
        {
            spray.Stop();
            spraySound.Stop();
        }

    }
    public void InteractableActivate(ActivateEventArgs args)
    {
        spraying = true;;
    }

    public void InteractableDeactivate(DeactivateEventArgs args)
    {
        spraying = false;
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
