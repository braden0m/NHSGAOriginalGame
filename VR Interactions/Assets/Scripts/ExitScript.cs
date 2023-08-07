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
            // we dont want double collision
            collision.gameObject.SetActive(false);
        }
    }
}
