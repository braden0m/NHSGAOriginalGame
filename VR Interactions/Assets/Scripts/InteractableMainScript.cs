using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteractableMainScript : MonoBehaviour
{

    public List<GameObject> allInteractables = new List<GameObject>();

    [SerializeField] private GameObject shardParticle;
    [SerializeField] private List<AudioClip> audioClips;
    //[SerializeField] private GameObject Handle;

    // Start is called before the first frame update
    void Start()
    {
        allInteractables = GameObject.FindGameObjectsWithTag("Breakable").ToList<GameObject>();

        foreach (GameObject value in allInteractables)
        {
            value.transform.parent = this.gameObject.transform;
            Material[] materialList = value.gameObject.GetComponent<MeshRenderer>().materials;

            //Debug.Log(materialList.Length - 2);

            //print(materialList.Length - 2);
            //Material objectMaterial = materialList[0];

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


            if (value.gameObject.GetComponent<AudioSource>() == null)
            {
                AudioSource newAudio = value.gameObject.AddComponent<AudioSource>();
                newAudio.maxDistance = 3;

                AudioClip selectedClip = audioClips[0];
                foreach (AudioClip clip in audioClips)
                {
                    if (clip.name == value.gameObject.GetComponent<MeshRenderer>().material.name + "Break")
                    {
                        selectedClip = clip;
                        break;
                    }
                }

                newAudio.clip = selectedClip;
            }

            if (value.gameObject.GetComponent<BreakableObject>() == null)
            {
                value.gameObject.AddComponent<BreakableObject>();
            }
        }
    }
}
