using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class SpaceShipController2 : MonoBehaviour
{
    //Translation
    private Vector3 velocity;
    private Vector3 position;
    private float translationSpeed;

    //Rotation
    public Vector3 rotation;
    private Vector3 rot_accel;
    private Vector3 rot_vel;
    private Vector3 balance_rot;
    private Vector3 balance_pos;

    private float k;
    private float rotationSpeed;
    private float maxRot;


    // Start is called before the first frame update
    void Start()
    {
        position = this.transform.position;

        rot_accel = new Vector3(0.0f, 0.0f, 0.0f);
        rot_vel = new Vector3(0.0f, 0.0f, 0.0f);

        translationSpeed = 10.0f;

        k = 100.0f;
        maxRot = 2.0f;
        rotationSpeed = 15.0f;

        balance_rot = this.transform.position + this.transform.forward * 5.0f;
        balance_pos = this.transform.position;
        rotation = balance_rot;
    }

    // Update is called once per frame
    void Update()
    {

        float x = Input.GetAxis("Horizontal") * translationSpeed;
        float y = Input.GetAxis("Vertical") * translationSpeed;

        float rotX = Input.GetAxis("Horizontal") * rotationSpeed;
        float rotY = Input.GetAxis("Vertical") * rotationSpeed;

        velocity = new Vector3(x, y, 0.0f);
        position += velocity * Time.deltaTime;
        this.transform.position = position;   

        Vector3 dir = rotation - balance_rot;
        rot_accel = -k * dir;

        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            rot_vel.x += rot_accel.x * Time.deltaTime;
            if (rotation.x < 0.5 && rotation.x > -0.5)
            {
                rot_vel.x = 0.0f;
                rotation.x = 0.0f;
            }
        }else
        {
            rot_vel.x = rotX;
        }
        rotation.x += rot_vel.x * Time.deltaTime;
        rotation.x = Mathf.Clamp(rotation.x, -maxRot, maxRot);

        if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
        {
            rot_vel.y += rot_accel.y * Time.deltaTime;
            if (rotation.y < 0.5 && rotation.y > -0.5)
            {
                rot_vel.y = 0.0f;
                rotation.y = 0.0f;
            }
        }
        else
        {
            rot_vel.y = rotY;
        }
        rotation.y += rot_vel.y * Time.deltaTime;
        rotation.y = Mathf.Clamp(rotation.y, -maxRot, maxRot);

        this.transform.forward = rotation - balance_pos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position + rotation, 0.5f);
    }

}
