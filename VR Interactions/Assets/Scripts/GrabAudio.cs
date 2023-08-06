using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabAudio : MonoBehaviour
{

    public AudioSource grabSound;

    public void InteractableHeld(SelectEnterEventArgs args)
    {
        grabSound.Play();
    }
    public void InteractableUnHeld(SelectEnterEventArgs args)
    {
        grabSound.Play();
    }

}
