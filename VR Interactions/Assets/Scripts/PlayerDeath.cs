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
    public VolumeProfile volumeProfile;
    private Vignette vignetteProfile;
    private float playerAge;
    private bool stayInFire;
    private float lerpTime;

    [SerializeField] Color originalColor, newColor;

    void Start()
    {
        if (volumeProfile.TryGet<Vignette>(out vignetteProfile))
        {
            //Debug.Log("Got it");
        }
        playerAge = 0f;
        stayInFire = false;
        lerpTime = 5f;
    }

    public void ChangeVignette()
    {
        //if (lerpTime / 5f<1.0f)
        lerpTime += Time.deltaTime;
        //else
        //{
        //    lerpTime -= Time.deltaTime;
        //}
        float newIntensity = Mathf.Lerp(0.7f, 1.0f, (float)Math.Sin(lerpTime)/2f+0.5f);
        vignetteProfile.intensity = new ClampedFloatParameter(newIntensity, 0.7f, 1f);
        
        
    }

    private void Update()
    {
        if (stayInFire)
        {
            // began lerping the intensity
            ChangeVignette();
        }
        else
        {
            vignetteProfile.active = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        // ! add tag
        if (!vignetteProfile.IsActive())
        {
            vignetteProfile.active = true;
        }
        if (playerAge > 20f)
        {
            Debug.Log("Player DIE");
            // want this to be only called once.
            //StartCoroutine(Die());
        }
        
        if (other.gameObject.tag == "Fire")
        {
            playerAge += Time.deltaTime;
            vignetteProfile.color = new ColorParameter(Color.Lerp(originalColor, newColor, playerAge / 15f));
            Debug.Log(playerAge);
            stayInFire = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Fire")
        {
            //playerAge += Time.deltaTime;
            //Debug.Log(playerAge);
            stayInFire = false;
        }
        Debug.Log("hha");
    }
}