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
    [SerializeField] private RectTransform waterLevelBar;

    [SerializeField] private float waterlevel;

    private bool held = false;
    private bool spraying = false;

    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<XRBaseInteractable>();
        cubeRenderer = GetComponent<Renderer>();

        waterlevel = 1;
    }
    private void FixedUpdate()
    {
        waterLevelBar.localPosition = new Vector3(0, (1 - waterlevel) / 2, 0);
        waterLevelBar.localScale = new Vector3(1, waterlevel, 1);

        if (spraying && waterlevel > 0)
        {
            waterlevel -= 0.01f;

            RaycastHit2D rayhit = Physics2D.Raycast(spray.transform.position, spray.transform.forward, 5);

            if (rayhit.collider != null && rayhit.collider.gameObject.CompareTag("Fire"))
            {
                Destroy(rayhit.collider.gameObject);
            }

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
