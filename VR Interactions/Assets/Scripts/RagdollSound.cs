using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RagdollSound : MonoBehaviour
{
    public AudioSource dragRag;
    public AudioSource miSound;

    public void InteractableHeld(SelectEnterEventArgs args)
    {
        miSound.Stop();
        dragRag.Play();
    }

    
}
