using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragRag : MonoBehaviour
{
    public Transform hand;
    // drag
    public bool moveX, moveY, moveZ;

    private GameObject curRag;
    private bool isDrag;

    void Start()
    {
        
    }

    void Update()
    {
        Debug.Log(curRag);
        //Debug.Log(isDrag);
        if (isDrag && curRag != null)
        {
            Debug.Log("grabbing");
            curRag.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            curRag.transform.position = hand.position + new Vector3(0, -0.5f, 0);
            //Debug.Log(curRag);
        }
    }

    void OnDrag(InputValue action)
    {
        isDrag = (action.Get<float>() == 1);

        /*
        //// if trigger and can drag
        if (curRag != null)
        {
            isDrag = true;
        //    if (!isDrag)
        //    {
        //        curRag = null;
        //    }
        }
        else if (isDrag)
        {
            isDrag = false;
        }
        //Debug.Log(curRag);

        */
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hand touch ");
        if (other.gameObject.tag == "Rag")
        {
            
            curRag = other.gameObject;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("hand leave ");
        if (other.gameObject.tag == "Rag")
        {
            isDrag = false;
            curRag = null;

        }
    }


}
