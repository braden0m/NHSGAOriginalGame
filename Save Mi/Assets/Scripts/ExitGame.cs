using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExitGame : MonoBehaviour
{

    public TextMeshProUGUI textbox;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hand touch ");
        if (other.gameObject.tag == "Rag")
        {

            textbox.text = "Success";

        }
    }

}
