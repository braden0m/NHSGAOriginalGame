using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private GameObject[] ragdolls;
    private int deadRagdolls, savedRagdolls;
    public Canvas endScreen;
    public TextMeshProUGUI endText;
    void Start()
    {
        ragdolls = GameObject.FindGameObjectsWithTag("Ragdoll");
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
       
        endScreen.enabled = true;

    }
}
