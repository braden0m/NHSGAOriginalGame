using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MenuStart : MonoBehaviour
{
    private float timer = 0f;
    [SerializeField] private float heldTimeRequired;
    private bool held = false;
    private Renderer cubeRenderer;
    [SerializeField] private Color inactiveColor;
    [SerializeField] private Color activeColor;

    // Start is called before the first frame update
    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        cubeRenderer.material.color = Color.Lerp(inactiveColor, activeColor, timer/heldTimeRequired);
        Debug.Log(held);
        if (held)
        {
            timer += Time.deltaTime;
            Debug.Log(timer);
            if (timer > heldTimeRequired)
            {
                StartGame();
            }
        }
    }

    public void InteractableHover(HoverEnterEventArgs args)
    {
        held = true;
    }
    public void InteractableunHover(HoverExitEventArgs args)
    {
        held = false;
        timer = 0;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
