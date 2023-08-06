using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void StartGame()
    {
        Debug.Log("START GAME");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);

    }
}
