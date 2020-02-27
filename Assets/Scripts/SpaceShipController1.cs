using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class SpaceShipController1 : MonoBehaviour
{
    private Vector3 velocity;
    private Vector3 position;

    public Vector3 rotation;
    private Vector3 rot_accel;
    private Vector3 rot_vel;


    private float translationSpeed;

    float k;
    float rot_stepX, rot_stepY;
    

    // Start is called before the first frame update
    void Start()
    {
        position = this.transform.position;

        rotation = new Vector3(0.0f, 0.0f, 0.0f);
        rot_accel = new Vector3(0.0f, 0.0f, 0.0f);
        rot_vel = new Vector3(0.0f, 0.0f, 0.0f);

        translationSpeed = 10.0f;

        k = 10.0f;
        rot_stepX = 7.0f;
        rot_stepY = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {

        float x = Input.GetAxis("Horizontal") * translationSpeed;
        float y = Input.GetAxis("Vertical") * translationSpeed;

        float rotX = Input.GetAxis("Horizontal") * rot_stepX;
        float rotY = Input.GetAxis("Vertical") * rot_stepY;

        velocity = new Vector3(x, y, 0.0f);
        position += velocity * Time.deltaTime;
        this.transform.position = position;

        rot_accel = -k * rotation;
        rotation.z -= rotX;
        rotation.x -= rotY;

        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            rot_vel.z += rot_accel.z * Time.deltaTime;
            rotation.z += rot_vel.z * Time.deltaTime;
            if (rotation.z < 2 && rotation.z > -2)
            {
                rot_vel.z = 0.0f;
                rotation.z = 0.0f;
            }
        }

        if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
        {
            rot_vel.x += rot_accel.x * Time.deltaTime;
            rotation.x += rot_vel.x * Time.deltaTime;
            if (rotation.x < 2 && rotation.x > -2)
            {
                rot_vel.x = 0.0f;
                rotation.x = 0.0f;
            }
        }

        rotation.z = Mathf.Clamp(rotation.z, -30.0f, 30.0f);
        rotation.x = Mathf.Clamp(rotation.x, -20.0f, 5.0f);

        this.transform.rotation = Quaternion.Euler(rotation);
    }



}
