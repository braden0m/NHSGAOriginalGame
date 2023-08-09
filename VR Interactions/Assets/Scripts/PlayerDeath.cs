using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SocialPlatforms;
using static UnityEngine.Rendering.DebugUI;

public class PlayerDeath : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject globalVolume;
    private Vignette vignetteProfile;
    private float playerAge;
    private bool stayInFire;
    private float lerpTime, addTime, newIntensity;

    [SerializeField] Color originalColor, newColor;

    public delegate void playerDeath();
    public static event playerDeath OnDeath;

    void Start()
    {
        addTime = 1f;
        playerAge = 0f;
        stayInFire = false;
        lerpTime = 5f;
    }

    public void ChangeVignette()
    {
        if (newIntensity > 1.0f || newIntensity < 0f)
            addTime = -addTime;
        newIntensity += addTime*Time.deltaTime;
        //float newIntensity = Mathf.Lerp(0f, 1.0f, (float)Math.Sin(lerpTime)/2f+0.5f);
        VolumeManager.instance.stack.GetComponent<Vignette>().intensity = new ClampedFloatParameter(newIntensity, 0f, 1f, true);
        Debug.Log(newIntensity);
    }

    private void Update()
    {
        if (stayInFire)
        {
            //Debug.Log(playerAge);
            playerAge += Time.deltaTime;
            VolumeManager.instance.stack.GetComponent<Vignette>().color = new ColorParameter(Color.Lerp(originalColor, newColor, playerAge / 10f), true);
            ChangeVignette();
        }
        else
        {
            VolumeManager.instance.stack.GetComponent<Vignette>().intensity = new ClampedFloatParameter(0, 0, 1f, true);
        }
    }
    //    if (stayInFire)
    //    {
    //        vignetteProfile.active = true;
    //        // began lerping the intensity
    //        //ChangeVignette();
    //    }
    //    else
    //    {
    //        vignetteProfile.active = false;
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Fire")
        {
            //VolumeManager.instance.stack.GetComponent<Vignette>().intensity = new ClampedFloatParameter(1, 0, 1f, true);

            Debug.Log("BURN");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        // ! add tag
        //if (!vignetteProfile.IsActive())
        //{
        //    vignetteProfile.active = true;
        //}

        if (playerAge > 20f)
        {
            Debug.Log("Player DIE");
            VolumeManager.instance.stack.GetComponent<Vignette>().intensity = new ClampedFloatParameter(1, 0, 1f, true);

            //if (OnDeath != null)
            //{
            //    GameManager.instance.OnDeath();
            //}

            GameManager.instance.OnDeath();
            return;
            // want this to be only called once.
            //StartCoroutine(Die());
        }
        if (other.gameObject.tag == "Fire")
        {
            stayInFire = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Fire")
        {
            //VolumeManager.instance.stack.GetComponent<Vignette>().intensity = new ClampedFloatParameter(0, 0, 1f, true);
            //playerAge += Time.deltaTime;
            //Debug.Log(playerAge);
            stayInFire = false;
            Debug.Log("NO BURN");
        }
        
    }
}