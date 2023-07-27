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

    Vector2 eyeStartingPosition;
    public GameObject openEye;
    public GameObject closedEye;
    public GameObject whiteBackground;
    public GameObject blackBackground;
    public TextMeshProUGUI proximityWarning;
    public Canvas canvas;
    public TextMeshProUGUI lastSeenCounter;

    int heatVisionEnemyCount = 0;
    List<GameObject> heatVisionEnemies = new List<GameObject>();
    public GameObject enemyIcon;
    List<GameObject> heatVisionEnemyIcons = new List<GameObject>();

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

        if (cloakAbility == 1)
        {
            //proximity detection
            RaycastHit proximityRay;
            Physics.Raycast(transform.position, rb.velocity, out proximityRay, rb.velocity.magnitude * 1.5f);

            if (proximityRay.collider != null)
            {
                proximityWarning.gameObject.SetActive(true);
                proximityWarning.transform.position = currentCamera.WorldToScreenPoint(proximityRay.point);
                proximityWarning.text = "Proximity Warning: " + (proximityRay.point - transform.position).magnitude + " units away.";

            }
            else
            {
                proximityWarning.gameObject.SetActive(false);
            }
        }
        else
        {
            proximityWarning.gameObject.SetActive(false);
        }

        //Enemy Vision Ray

        if (isDetectedByEnemy() != null)
        {
            if (cloakAbility == 0f)
            {
                lastSeenElapsedTime = 0f;
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

        //Jumping
        if (!midair && isJumping == 1f)
        {
            midair = true;
            rb.AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
        }

        if (heatVision == 1f)
        {
            Time.timeScale = 0.3f;
            cameraSpeed = 0.1f;

            openEye.transform.position = mouse.position.ReadValue();
            whiteBackground.gameObject.SetActive(true);

            heatVisionEnemies = allHeatVisionEnemiesInRange(75, 500);

            if (heatVisionEnemies.Count != heatVisionEnemyCount)
            {
                updateHeatVision();
            }

            heatVisionEnemyCount = heatVisionEnemies.Count;

            if (heatVisionEnemyIcons.Count > 0) //&& heatVisionEnemies.Count == heatVisionEnemyIcons.Count)
            {
                print(heatVisionEnemyIcons.Count);

                for (int i = 0; i < heatVisionEnemyCount; i++)
                {
                    //print("Updating");
                    heatVisionEnemyIcons[i].transform.position = currentCamera.WorldToScreenPoint(heatVisionEnemies[i].transform.position);
                    //float scaleFactor = 1 / (heatVisionEnemies[i].transform.position - transform.position).magnitude;
                    //heatVisionEnemyIcons[i].transform.localScale = new Vector3(scaleFactor, scaleFactor, 0);
                }
            }
        }
        else
        {
            Time.timeScale = 1f;
            cameraSpeed = 0.7f;
            enemyIcon.gameObject.SetActive(false);
            openEye.transform.position = eyeStartingPosition;
            whiteBackground.gameObject.SetActive(false);

            if (heatVisionEnemyIcons.Count > 0)
            {
                foreach (GameObject val in heatVisionEnemyIcons)
                {
                    Destroy(val);
                }
            }
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

    List<GameObject> allHeatVisionEnemiesInRange(float dist3D, float dist2D)
    {
        List<GameObject> result = new List<GameObject>();

        foreach (GameObject val in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            float distanceDifference = (val.transform.position - transform.position).magnitude;

            if (distanceDifference <= dist3D)
            {
                if ((val.transform.position - (transform.position + currentCamera.transform.forward)).magnitude < distanceDifference)
                {
                    if (((Vector2)currentCamera.WorldToScreenPoint(val.transform.position) - (Vector2)mouse.position.ReadValue()).magnitude < dist2D)
                    {
                        result.Add(val);
                    }

                }
            }
        }

        return result;
    }
    void updateHeatVision()
    {
        if (heatVisionEnemyIcons.Count > 0)
        {
            foreach (GameObject val in heatVisionEnemyIcons)
            {
                Destroy(val);
            }
            heatVisionEnemyIcons.Clear();
        }


        foreach (GameObject enemy in heatVisionEnemies)
        {
            GameObject marker = Instantiate(enemyIcon, currentCamera.WorldToScreenPoint(enemy.transform.position), Quaternion.identity, canvas.transform);
            marker.gameObject.SetActive(true);
            heatVisionEnemyIcons.Add(marker);
        }
    }
    GameObject isDetectedByEnemy()
    {
        GameObject result = null;

        foreach (GameObject val in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            RaycastHit enemyRay;

            if (Physics.Raycast(val.transform.position, (transform.position - val.transform.position), out enemyRay, 75f))
            {

                if (enemyRay.collider != null && enemyRay.collider.gameObject.CompareTag("Player"))
                {
                    result = val;
                    break;
                }
            }
        }

        return result;
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

            if (heatVision == 1)
            {
                updateHeatVision();
            }
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
