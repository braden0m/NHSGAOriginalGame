using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

public class JoystickMovement : MonoBehaviour
{
    public Rigidbody player;
    public float speed;

    void Start()
    {
        
    }

    void Update()
    {

        Vector2 joystickDirection = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick); //Or maybe secondary thumbstick?

        //var joystick = OVRInput.Get(OVRInput.Get(OVRInput.RawAxis2D.LThumbstick, OVRInput.Controller.LTouch));
        float posY = player.position.y;

        //player.transform.position += transform.forward * joystick.y * speed * Time.deltaTime;
        //player.transform.position = new Vector3(player.transform.position.x, player.transform.position.x, player.transform.position.x);

        player.transform.position += Quaternion.Euler(0, player.rotation.eulerAngles.y, 0) * Vector3.forward;
    }
}
