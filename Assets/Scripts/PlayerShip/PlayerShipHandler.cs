using System;
using UnityEngine;

public class PlayerShipHandler : MonoBehaviour
{

    private Vector3 screenBoundaries;
    private Inputs inputs;  //Obtener controles

    // Translation
    public Vector2 t_velocity;
    public Vector2 position;
    private float t_acceleration = 120.0f;
    private float t_deceleration = 40.0f;
    private float t_maxVelocity = 15.0f;

    // Rotation
    public Vector2 r_velocity;
    public Vector2 rotation = Vector2.zero;
    public Vector3 rest_rotation;
    private float r_acceleration = 1000.0f;
    private float r_maxVelocity = 500.0f;
    private float r_maxRotationX = 20.0f;
    private float r_maxRotationY = 15.0f;
    private float r_stiffness = 20.0f;

    //Bullet
    public GameObject bullet;
    public float bulletSpeed = 20.0f;
    public float bulletLifeTime = 2.0f;

    //Boundary
    private float radius;
    public Curve curve;

    //Score control
    public ScoreManager scoreManager;

    //Game manager
    public GameManager gameManager;

    void Start()
    {
        position = this.transform.position;
        //rotation = Vector2.zero;
        //res_rotation = this.transform.rotation.eulerAngles;

        t_velocity = Vector2.zero;
        r_velocity = Vector2.zero;

        //Camera must be in (0, 0, z)
        screenBoundaries = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        screenBoundaries.x = Math.Abs(screenBoundaries.x);
        screenBoundaries.y = Math.Abs(screenBoundaries.y);

        //Obtener controles para ordenador o para moviles
        StandaloneInput standaloneInput = this.gameObject.AddComponent<StandaloneInput>();
        inputs = (Inputs)standaloneInput;

        /*
        #if UNITY_ANDROID
            PhoneInputs phoneInputs = new PhoneInputs();
            inputs = (Inputs)phoneInputs;
            inputs.setJoystick(joystick);
        #else
            StandaloneInput standaloneInput = new StandaloneInput();
            inputs = (Inputs)standaloneInput;
        #endif
        */

        radius = curve.pipeRadius - 2.0f;
    }

    void Update()
    {
        if (!gameManager.paused)
        {
            // Getting buttons pressed (obtains 0, 1 or -1)
            float x = inputs.GetInputX();
            float y = inputs.GetInputY();

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
            if (position.magnitude > radius)
                position = position.normalized * radius;
            //position.x = Mathf.Clamp(position.x, -screenBoundaries.x + 1, screenBoundaries.x - 1);
            //position.y = Mathf.Clamp(position.y, -screenBoundaries.y + 1, screenBoundaries.y - 1);

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


            if (Input.GetKeyUp(KeyCode.J))
                shoot();


            if (Input.GetKeyUp(KeyCode.K))
                print("Power Up");
        }
    }

    private void updateTransform()
    {
        
        this.transform.position = /*Quaternion.Euler(rest_rotation) **/ position;

        // Using vertical rotation with x axes and horizontal rotation with y and z axes
        this.transform.rotation = Quaternion.Euler(new Vector3(-rotation.y, rotation.x * 0.5f, -rotation.x) /*+ rest_rotation*/);
        
        
    }

    private void shoot()
    {
        Bullet goBullet = DynamicPool.instance.GetObj(DynamicPool.objType.Bullet).GetComponent<Bullet>();
        goBullet.transform.position = this.transform.position;
        goBullet.transform.rotation = Quaternion.identity;
        goBullet.Shoot(bulletSpeed, bulletLifeTime);
    }

    public void OnTriggerEnter(Collider other)
    {
        DynamicPool.objType obstacle = (DynamicPool.objType)Enum.Parse(typeof(DynamicPool.objType), other.gameObject.name);

        other.isTrigger = false;

        switch (obstacle)
        {
            case DynamicPool.objType.Asteroid:
                scoreManager.LooseBarrel();
                break;

            case DynamicPool.objType.PortalBarrel:
                scoreManager.EarnBarrel();
                break;

            case DynamicPool.objType.PortalPlanet:
                scoreManager.DistributeBarrel();
                break;
            case DynamicPool.objType.Bullet:
                //Bullets have rigidbodies and should keep being istrigger
                other.isTrigger = true;
                break;
        }
    }

}

