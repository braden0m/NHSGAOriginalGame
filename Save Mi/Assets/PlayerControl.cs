using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

//READ ME
//Please note: Some public variable references might be broken after exporting
//Please email me at dgao@andrew.cmu.edu if this happens

public class PlayerControl: MonoBehaviour
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

    public float cameraSpeed;
    public float maxRotation = 75f;
    public Camera currentCamera;
    Vector2 deltaLookDirection;

    Mouse mouse;

    // Start is called before the first frame update
    void Start()
    {
        mouse = Mouse.current;
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

        if (Physics.Raycast(transform.position - new Vector3(0, 1.7f, 0), -transform.up, out midAirRay, 1f))
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


        //Jumping
        if (!midair && isJumping == 1f)
        {
            midair = true;
            rb.AddForce(new Vector3(0, 1, 0), ForceMode.Impulse);
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

    void OnJump(InputValue action)
    {
        isJumping = action.Get<float>();
        Debug.Log(isJumping);
    }
}
