using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections;
using System.Collections.Generic;

public class PlayerShipHandler : MonoBehaviour
{
    public FloatingJoystickVersion joystick;
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

    //Explosion
    public Explosion explosionHandler;

    //Invincibility
    public bool invincibility = false;

    //Handling PowerUps
    public List<GameObject> powerUpsInterface;
    public int powerUpID;
    public bool hasPowerUp = false;

    //Boundary
    [HideInInspector]
    public float radius;

    //Score control
    [HideInInspector]
    public ScoreManager scoreManager;

    //Game manager
    [HideInInspector]
    public GameManager gameManager;

    //Soundmanager
    [HideInInspector]
    public SoundManager soundManager;

    [HideInInspector]
    public UIManager uiManager;

    //Renderers of the spaceship
    Renderer[] renders;

    void Awake()
    {
        position = this.transform.position;
        //rotation = Vector2.zero;
        //res_rotation = this.transform.rotation.eulerAngles;

        t_velocity = Vector2.zero;
        r_velocity = Vector2.zero;


        //Asegurarse de que la nave hija se llame Playership
        renders = this.transform.Find("PlayerShip").GetComponentsInChildren<Renderer>();


        //Obtener controles para ordenador o para moviles
        StandaloneInput standaloneInput = this.gameObject.AddComponent<StandaloneInput>();
        inputs = (Inputs)standaloneInput;
        
         #if UNITY_ANDROID
             PhoneInputs phoneInputs = this.gameObject.AddComponent<PhoneInputs>();
             inputs = (Inputs)phoneInputs;
             joystick.gameObject.SetActive(true);
             inputs.SetJoystick(joystick, this);
         #else
             joystick.gameObject.SetActive(false);
             standaloneInput = this.gameObject.AddComponent<StandaloneInput>();
             inputs = (Inputs)standaloneInput;
         #endif
         


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
                Shoot();


            if (Input.GetKeyUp(KeyCode.K))
                StartPowerUp();
        }
    }

    private void updateTransform()
    {
        this.transform.position = position;

        // Using vertical rotation with x axes and horizontal rotation with y and z axes
        this.transform.rotation = Quaternion.Euler(new Vector3(-rotation.y, rotation.x * 0.5f, -rotation.x));
    }

    public void Shoot()
    {
        Bullet goBullet = DynamicPool.instance.GetObj(DynamicPool.objType.Bullet).GetComponent<Bullet>();
        goBullet.transform.position = this.transform.position;
        goBullet.transform.rotation = Quaternion.identity;
        goBullet.Shoot(bulletSpeed, bulletLifeTime);
    }

    public void StartPowerUp()
    {
        if(hasPowerUp)
        {
            UIManager uiManager = gameManager.uiManager;
            //Update powerup in interface
            powerUpsInterface[powerUpID].SetActive(false);

            //Update power up state
            hasPowerUp = false;

            //PowerUp activated sign
            uiManager.ShowPowerUpIcon(uiManager.powerUpsIcons[powerUpID]);

            //PowerUp effect
            gameManager.PowerUp(powerUpID); //PowerUp receives id of power up
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        DynamicPool.objType obstacle = (DynamicPool.objType)Enum.Parse(typeof(DynamicPool.objType), other.gameObject.name);

        other.isTrigger = false;

        switch (obstacle)
        {
            case DynamicPool.objType.Asteroid:
                if (!invincibility)
                {
                    scoreManager.LooseBarrel();

                    if (scoreManager.barrels == -1)
                    {

                        explosionHandler.PlayFullExplosion();
                        soundManager.mainTheme.source.Stop();
                        soundManager.PlayFinalCrashSound();
                    }
                    else
                    {
                        explosionHandler.PlaySmokeExplosion();
                        soundManager.PlayCrashSound();
                        StartCoroutine(ProtectFromAsteroids(2));
                    }
                }

                break;

            case DynamicPool.objType.PortalBarrel:
                scoreManager.EarnBarrel();
                animatePortalCollision(other.gameObject);
                soundManager.PlayBeerPortal();
                break;

            case DynamicPool.objType.PortalPlanet:
                scoreManager.DistributeBarrel();
                animatePortalCollision(other.gameObject);
                soundManager.PlayPlanetPortal();
                break;

            case DynamicPool.objType.Bullet:
                //Bullets have rigidbodies and should keep being istrigger
                other.isTrigger = true;
                break;

            case DynamicPool.objType.PowerUpSnail:
                if (!hasPowerUp)
                {
                    hasPowerUp = true;
                    powerUpID = 0;
                    powerUpsInterface[powerUpID].SetActive(true);
                }
                soundManager.PlayPowerUpPortal();
                animatePortalCollision(other.gameObject);
                break;

            case DynamicPool.objType.PowerUpShield:
                if (!hasPowerUp)
                {
                    hasPowerUp = true;
                    powerUpID = 1;
                    powerUpsInterface[powerUpID].SetActive(true);
                }
                soundManager.PlayPowerUpPortal();
                animatePortalCollision(other.gameObject);
                break;
            
        }
    }

    public void animatePortalCollision(GameObject portal)
    {
        portal.transform?.DOScale(1.3f, 0.2f).OnComplete(() =>
        {
            portal.transform?.DOScale(0.0f, 0.5f);
        }
        );
    }

    IEnumerator ProtectFromAsteroids(int seconds)
    {
        invincibility = true;
        StartCoroutine(BlinkShip());
        yield return new WaitForSeconds(seconds);
        invincibility = false;
    }

    IEnumerator BlinkShip()
    {
        while (invincibility)
        {
            foreach (Renderer render in renders)
                render.enabled = false;
            yield return new WaitForSeconds(0.1f);

            foreach (Renderer render in renders)
                render.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }
}

