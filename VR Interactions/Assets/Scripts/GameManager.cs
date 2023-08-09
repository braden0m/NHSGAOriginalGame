using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private GameObject[] ragdolls;
    private int deadRagdolls, savedRagdolls;
    public GameObject endScreen;
    public AudioSource deathSound;

    public int currentLevel;
    private bool playerDied;

    [SerializeField] private GameStateSO gameStateSO;
    void Start()
    {
        instance = this;
        ragdolls = GameObject.FindGameObjectsWithTag("Ragdoll");
        gameStateSO.ragdollTotal = ragdolls.Length;
        Debug.Log(ragdolls.Length);
        deadRagdolls = 0;
        savedRagdolls = 0;

        playerDied = false;
    }

    private void Update()
    {
        if (savedRagdolls + deadRagdolls == gameStateSO.ragdollTotal) {
            GameEnd();
        }
    }

    public void RagdollLife()
    {
        deadRagdolls++;
        // play deathsound here since sometimes you haven't seen the ragdoll but they already died
        deathSound.Play();
        Debug.Log(deadRagdolls);
    }

    public void SaveRagdoll()
    {
        savedRagdolls++;
        Debug.Log(savedRagdolls);   
    }

    public void OnDeath()
    {
        //PlayerDeath.OnDeath += DeathEnding;
        playerDied = true;
        GameEnd();
    }
    
    private void GameEnd()
    {
        gameStateSO.ragdollSaved = savedRagdolls;
        gameStateSO.ragdollTotal = ragdolls.Length;
        gameStateSO.currentLevel = currentLevel;

        if (playerDied)
        {
            gameStateSO.gameCase = 4;
        }
        else if (deadRagdolls == ragdolls.Length)
        {
            gameStateSO.gameCase = 1;
        }
        else if (savedRagdolls == ragdolls.Length)
        {
            gameStateSO.gameCase = 3;
        }
        else if (savedRagdolls + deadRagdolls == ragdolls.Length)
        {
            gameStateSO.gameCase = 2;
        }

        SceneManager.LoadScene("WinLoseScene");
    }
}
