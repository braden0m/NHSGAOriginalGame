using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;
public class ToMenu : MonoBehaviour
{
    private float timer = 0f;
    [SerializeField] private float heldTimeRequired;
    private bool held = false;
    private Renderer cubeRenderer;
    [SerializeField] private Color inactiveColor;
    [SerializeField] private Color activeColor;

    [SerializeField] private GameStateSO gameStateSO;

    [SerializeField] private TextMeshProUGUI endText;

    // Start is called before the first frame update
    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();

        DisplayInfo(gameStateSO.gameCase);
    }

    // Update is called once per frame
    void Update()
    {
        cubeRenderer.material.color = Color.Lerp(inactiveColor, activeColor, timer / heldTimeRequired);

        if (held)
        {
            timer += Time.deltaTime;

            if (timer > heldTimeRequired)
            {
                GoToMenu();
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

    private void DisplayInfo(int caseNum)
    {
        switch (caseNum)
        {
            case 1:
                endText.text = $"You saved none of the {gameStateSO.ragdollTotal} ragdolls. Please don't be a firefighter.";
                break;
            case 2:
                endText.text = $"You saved {gameStateSO.ragdollSaved} out of the {gameStateSO.ragdollTotal} ragdolls. Try again!";
                break;
            case 3:
                endText.text = $"Congratulations! You saved all {gameStateSO.ragdollTotal} ragdolls. Good job!";
                break;
        }

    }
    public void GoToMenu()
    {
        SceneManager.LoadScene("StartScene");
    }
}
