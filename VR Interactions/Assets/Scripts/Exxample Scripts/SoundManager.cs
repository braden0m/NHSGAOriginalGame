using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource au_mixedfire;
    private AudioSource au_fullsong;

    private void Awake()
    {
        foreach (GameObject thing in GameObject.FindGameObjectsWithTag("Breakable"))
        {
            if (thing.GetComponent<AudioSource>() != null)
                thing.GetComponent<AudioSource>().enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        au_mixedfire = GetComponentsInChildren<AudioSource>()[0];
        au_fullsong = GetComponentsInChildren<AudioSource>()[1];

        if (au_mixedfire == null)
            Debug.LogWarning("can't find");
        else
        {
            Debug.LogWarning("good");
        }
            au_mixedfire.Play();

        au_fullsong.Play();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
