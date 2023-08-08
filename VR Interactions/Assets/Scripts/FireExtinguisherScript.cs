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
    [SerializeField] private ParticleSystem spray;
    [SerializeField] private AudioSource spraySound;
    [SerializeField] private GameObject waterDisplay;
    [SerializeField] private RectTransform waterLevelBar;

    [SerializeField] private float waterlevel;

    private bool held = false;
    private bool spraying = false;

    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<XRBaseInteractable>();
        cubeRenderer = GetComponent<Renderer>();
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

        } else
        {
            spray.Stop();
            spraySound.Stop();
        }

        /*
        if (!died && barrelHit.collider != null && barrelHit.collider.gameObject.CompareTag("Enemy") && !(skippedBarrels.Exists(x => x == barrelHit.collider.gameObject)))
        {
            StartCoroutine(ShowObtainedScore(barrelHit.point, 100, false));

            skippedBarrels.Add(barrelHit.collider.gameObject);
        }
        else if (died && barrelHit.collider != null && barrelHit.collider.gameObject.CompareTag("Bottom"))
        {
            if (!splashed)
            {
                splashed = true;
                Instantiate(splash, transform.position, Quaternion.identity);
            }
        }

        */
    }
    public void InteractableActivate(ActivateEventArgs args)
    {
        spraying = true;
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
