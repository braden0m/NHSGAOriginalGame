using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RagdollSound : MonoBehaviour
{
    public AudioSource dragRag;
    public AudioSource miSound;
    public AudioSource hurtSound;
    public void InteractableHeld(SelectEnterEventArgs args)
    {
        miSound.Stop();
        dragRag.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BreakTool")
        {
            hurtSound.Play();
        }
    }
}
