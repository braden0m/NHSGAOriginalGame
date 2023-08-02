using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private GameObject[] ragdolls;
    private int deadRagdolls, savedRagdolls;
    public GameObject endScreen;
    public TextMeshProUGUI endText;
    void Start()
    {
        instance = this;
        ragdolls = GameObject.FindGameObjectsWithTag("Ragdoll");
        Debug.Log(ragdolls.Length);
        deadRagdolls =0;
        savedRagdolls = 0;
    }

    public void RagdollLife()
    {
        deadRagdolls++;
        if (deadRagdolls == ragdolls.Length)
        {
            GameEnd(1);
        }
        else if (savedRagdolls + deadRagdolls == ragdolls.Length)
        {
            GameEnd(3);
        }
    }

    public void SaveRagdoll()
    {
        
        savedRagdolls++;
        Debug.Log(savedRagdolls);
        if (savedRagdolls == ragdolls.Length)
        {
            GameEnd(2);
        }
        else if (savedRagdolls + deadRagdolls == ragdolls.Length)
        {
            GameEnd(3);
        }
    
    }

    private void GameEnd(int caseNum)
    {
        switch (caseNum)
        {
            case 1:
                endText.text = "All ragdolls Died :(";
                break;
            case 2:
                endText.text = "All ragdolls saved:)";
                break;
            case 3:
                endText.text = "Some ragdolls died:(";
                break;
        }
        Debug.Log(caseNum);
        endScreen.SetActive(true);

    }
}
