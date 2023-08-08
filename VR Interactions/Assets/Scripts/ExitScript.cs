using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ragdoll")
        {
            Debug.Log("Collision");
            GameManager.instance.SaveRagdoll();
            collision.gameObject.transform.position = new Vector3(100f,100f,100f);
            // we dont want double collision
            //collision.gameObject.SetActive(false);
        }
    }
}
