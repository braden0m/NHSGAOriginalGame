using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteractableMainScript : MonoBehaviour
{

    public List<GameObject> allInteractables = new List<GameObject>();

    [SerializeField] private GameObject shardParticle;
    //[SerializeField] private GameObject Handle;

    // Start is called before the first frame update
    void Start()
    {
        allInteractables = GameObject.FindGameObjectsWithTag("Breakable").ToList<GameObject>();

        foreach (GameObject value in allInteractables)
        {
            value.transform.parent = this.gameObject.transform;

            if (value.gameObject.GetComponent<Rigidbody>() == null)
            {
                value.gameObject.AddComponent<Rigidbody>();
            }

            if (value.gameObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.XRGrabInteractable>() == null)
            {
                value.gameObject.AddComponent<UnityEngine.XR.Interaction.Toolkit.XRGrabInteractable>();
            }

            if (value.gameObject.GetComponentInChildren<ParticleSystem>() == null)
            {
                GameObject createdParticleObject = Instantiate(shardParticle, value.transform);

                ParticleSystem createdParticleSystem = createdParticleObject.GetComponent<ParticleSystem>();

                Renderer createdParticleRenderer = createdParticleSystem.GetComponent<Renderer>();

                createdParticleRenderer.material = value.gameObject.GetComponent<MeshRenderer>().material;
            }

            if (value.gameObject.GetComponent<BreakableObject>() == null)
            {
                value.gameObject.AddComponent<BreakableObject>();
            }
        }
    }
}
