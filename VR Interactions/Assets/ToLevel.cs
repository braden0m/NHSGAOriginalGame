using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class ToLevel : MonoBehaviour
{
    private float timer = 0f;
    [SerializeField] private float heldTimeRequired;
    private bool held = false;
    private Renderer cubeRenderer;
    [SerializeField] private Color inactiveColor;
    [SerializeField] private Color activeColor;

    [SerializeField] private GameStateSO gameStateSO;

    // Start is called before the first frame update
    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();

        //Temp fail case
        if (gameStateSO.ragdollSaved / gameStateSO.ragdollTotal < 0.5f)
        {
            Destroy(this.gameObject);
        }
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
                ToNextLevel();
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

    public void ToNextLevel()
    {
        SceneManager.LoadScene("Level" + gameStateSO.currentLevel);
    }
}
