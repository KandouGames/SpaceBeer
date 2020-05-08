using System;
using UnityEngine;

public class PhoneController : MonoBehaviour
{
    private Vector3 screenBoundaries;
    public Joystick joystick;

    // Translation
    public Vector2 t_velocity;
    public Vector2 position;
    private float t_acceleration = 80.0f;
    private float t_deceleration = 40.0f;
    private float t_maxVelocity = 20.0f;

    // Rotation
    public Vector2 r_velocity;
    public Vector2 rotation;
    private float r_acceleration = 1000.0f;
    private float r_maxVelocity = 500.0f;
    private float r_maxRotationX = 20.0f;
    private float r_maxRotationY = 15.0f;
    private float r_stiffness = 20.0f;

    void Start()
    {
        position = this.transform.position;
        rotation = new Vector2(0, 0);

        //Camera must be in (0, 0, z)
        screenBoundaries = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        screenBoundaries.x = Math.Abs(screenBoundaries.x);
        screenBoundaries.y = Math.Abs(screenBoundaries.y);
    }

    void Update()
    {
        // Getting buttons pressed (obtains 0, 1 or -1)
        float x, y;
        if (joystick.Horizontal >= 0.4f)
            x = joystick.Horizontal;
        else if (joystick.Horizontal <= -0.4f)
            x = joystick.Horizontal;
        else
            x = 0;

        if (joystick.Vertical >= 0.4f)
            y = joystick.Vertical;
        else if (joystick.Vertical <= -0.4f)
            y = joystick.Vertical;
        else
            y = 0;

        // Updating velocity in traslation
        t_velocity.x += t_acceleration * x * Time.deltaTime;
        t_velocity.y += t_acceleration * y * Time.deltaTime;
        t_velocity.x = Mathf.Clamp(t_velocity.x, -t_maxVelocity, t_maxVelocity);
        t_velocity.y = Mathf.Clamp(t_velocity.y, -t_maxVelocity, t_maxVelocity);

        // Updating velocity in rotation using acceleration created to recover rest position 
        // and acceleration created when pressing 
        float r_accelx = (-r_stiffness * rotation.x) + (r_acceleration * x);
        float r_accely = (-r_stiffness * rotation.y) + (r_acceleration * y);

        r_velocity.x += r_accelx * Time.deltaTime;
        r_velocity.y += r_accely * Time.deltaTime;
        r_velocity.x = Mathf.Clamp(r_velocity.x, -r_maxVelocity, r_maxVelocity);
        r_velocity.y = Mathf.Clamp(r_velocity.y, -r_maxVelocity, r_maxVelocity);

        // Horizontal
        if (x == 0)
        {
            // Decelerating velocity in traslation
            if (t_velocity.x != 0)
                if (t_velocity.x < 0.5 && t_velocity.x > -0.5)
                    t_velocity.x = 0;
                else
                    t_velocity.x -= t_deceleration * Time.deltaTime * (t_velocity.x / Math.Abs(t_velocity.x));

            // Forcing to stop rotation
            if (rotation.x < 3 && rotation.x > -3)
            {
                rotation.x = 0;
                r_velocity.x = 0;
            }
        }

        // Vertical
        if (y == 0)
        {
            // Decelerating velocity in traslation
            if (t_velocity.y != 0)
                if (t_velocity.y < 0.5 && t_velocity.y > -0.5)
                    t_velocity.y = 0;
                else
                    t_velocity.y -= t_deceleration * Time.deltaTime * (t_velocity.y / Math.Abs(t_velocity.y));

            // Forcing to stop rotation
            if (rotation.y < 3 && rotation.y > -3)
            {
                rotation.y = 0;
                r_velocity.y = 0;
            }
        }

        // Euler integration
        position += t_velocity * Time.deltaTime;
        rotation += r_velocity * Time.deltaTime;

        // Forcing position to stay on screen
        position.x = Mathf.Clamp(position.x, -screenBoundaries.x + 2, screenBoundaries.x - 2);
        position.y = Mathf.Clamp(position.y, -screenBoundaries.y + 1, screenBoundaries.y - 1);

        // Forcing to not rotate more than max
        if (rotation.x > r_maxRotationX || rotation.x < -r_maxRotationX)
        {
            rotation.x = Mathf.Clamp(rotation.x, -r_maxRotationX, r_maxRotationX);
            r_velocity.x = 0;
        }

        if (rotation.y > r_maxRotationY || rotation.y < -r_maxRotationY)
        {
            rotation.y = Mathf.Clamp(rotation.y, -r_maxRotationY, r_maxRotationY);
            r_velocity.y = 0;
        }


        updateTransform();


        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    private void updateTransform()
    {
        this.transform.position = position;

        // Using vertical rotation with x axes and horizontal rotation with y and z axes
        this.transform.rotation = Quaternion.Euler(new Vector3(-rotation.y, rotation.x * 0.5f, -rotation.x));
    }

}

