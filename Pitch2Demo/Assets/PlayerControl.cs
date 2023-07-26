using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

//READ ME
//Please note: Some public variable references might be broken after exporting
//Please email me at dgao@andrew.cmu.edu if this happens

public class PlayerControl : MonoBehaviour
{
    Rigidbody rb;

    public float moveSpeed;
    Vector3 moveDirection;
    float speedMultiplier = 1f;
    float isJumping = 0;
    float heatVision = 0;
    float cloakAbility = 0;
    float lastSeenElapsedTime = 0;
    bool midair = true;
    public GameObject enemy;

    Vector2 eyeStartingPosition;
    public GameObject openEye;
    public GameObject closedEye;
    public GameObject whiteBackground;
    public GameObject blackBackground;
    public GameObject enemyIcon;
    public TextMeshProUGUI lastSeenCounter;

    public float cameraSpeed;
    public float maxRotation = 75f;
    public Camera currentCamera;
    Vector2 deltaLookDirection;

    Mouse mouse;

    // Start is called before the first frame update
    void Start()
    {
        mouse = Mouse.current;
        eyeStartingPosition = closedEye.transform.position;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        lastSeenElapsedTime += Time.deltaTime;

        //Movement
        rb.AddForce((currentCamera.transform.forward * moveDirection.y + currentCamera.transform.right * moveDirection.x) * speedMultiplier * moveSpeed * Time.deltaTime, ForceMode.Force);
        //transform.rotation = Quaternion.LookRotation(currentCamera.transform.forward);


        //CameraLook
        float lookDirectionX;

        lookDirectionX = Mathf.Clamp(deltaLookDirection.y * cameraSpeed, -maxRotation, maxRotation);
        lookDirectionX = currentCamera.transform.eulerAngles.x - lookDirectionX;

       
        float lookDirectionY = currentCamera.transform.eulerAngles.y + deltaLookDirection.x * cameraSpeed;

        currentCamera.transform.eulerAngles = new Vector3(lookDirectionX, lookDirectionY, 0);

        //midair detection
        RaycastHit midAirRay;

        if (Physics.Raycast(transform.position - new Vector3(0, 1.1f, 0), -transform.up, out midAirRay, 1f))
        {
            //print(midAirRay.collider.gameObject.name);

            if (midAirRay.collider != null)
            {
                midair = false;
            }
            else
            {
                midair = true;
            }
        }

        //Enemy Vision Ray

        RaycastHit enemyRay;

        if (Physics.Raycast(enemy.transform.position, (transform.position - enemy.transform.position), out enemyRay, 100f))
        {
            //print(midAirRay.collider.gameObject.name);

            if (enemyRay.collider != null && enemyRay.collider.gameObject.CompareTag("Player"))
            {
                lastSeenElapsedTime = 0f;
                if (cloakAbility == 0f)
                {
                    lastSeenCounter.text = "Currently being seen by the enemy.";
                }
                else
                {
                    lastSeenCounter.text = "You would be currently visible to the enemy if you are not cloaking.";
                }
            }
            else
            {
                lastSeenCounter.text = "Last seen by enemy " + Mathf.Round(lastSeenElapsedTime * 100) / 100 + " seconds ago.";
            }
        }

        //Jumping
        if (!midair && isJumping == 1f)
        {
            midair = true;
            rb.AddForce(new Vector3(0, 1, 0), ForceMode.Impulse);
        }

        if (heatVision == 1f)
        {
            Time.timeScale = 0.3f;
            cameraSpeed = 0.1f;
            openEye.transform.position = mouse.position.ReadValue();
            whiteBackground.gameObject.SetActive(true);

            if (((Vector2) currentCamera.WorldToScreenPoint(enemy.transform.position) - (Vector2) mouse.position.ReadValue()).magnitude < 500)
            {
                enemyIcon.transform.position = currentCamera.WorldToScreenPoint(enemy.transform.position);
                enemyIcon.SetActive(true);
            }
            else
            {
                enemyIcon.SetActive(false);
            }
        }
        else
        {
            Time.timeScale = 1f;
            cameraSpeed = 0.7f;
            enemyIcon.gameObject.SetActive(false);
            openEye.transform.position = eyeStartingPosition;
            whiteBackground.gameObject.SetActive(false);
        }

        if (cloakAbility == 1f)
        {
            closedEye.gameObject.SetActive(true);
            openEye.gameObject.SetActive(false);
            blackBackground.gameObject.SetActive(true);
        } 
        else
        {
            openEye.gameObject.SetActive(true);
            closedEye.gameObject.SetActive(false);
            blackBackground.gameObject.SetActive(false);
        }
    }

    void OnMove(InputValue action)
    {
        moveDirection = action.Get<Vector2>();
    }
    void OnLook(InputValue action)
    {
        deltaLookDirection = action.Get<Vector2>();
    }

    void OnFire(InputValue action)
    {
        if (cloakAbility == 0f)
        {
            heatVision = action.Get<float>();
        }
    }

    void OnFire2(InputValue action)
    {
        if (heatVision == 0f)
        {
            cloakAbility = action.Get<float>();
        }
    }

    void OnJump(InputValue action)
    {
        isJumping = action.Get<float>();
    }
}
