using UnityEngine;
using System.Collections;

public class WarpSpeed : MonoBehaviour
{
    public float warpDistortion;
    public float particleSpeed;
    ParticleSystem particles;
    ParticleSystemRenderer rend;
    bool isWarping;

    public GameManager gameManager;

    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        rend = particles.GetComponent<ParticleSystemRenderer>();
        gameManager.onDifficultyChange += ChangeVelocity;
    }

    void Update()
    {
        if (isWarping && !atWarpSpeed())
        {
            rend.velocityScale += warpDistortion * (Time.deltaTime * particleSpeed);
        }

        if (!isWarping && !atNormalSpeed())
        {
            rend.velocityScale -= warpDistortion * (Time.deltaTime * particleSpeed);
        }
    }

    public void Engage()
    {
        isWarping = true;
    }

    public void Disengage()
    {
        isWarping = false;
    }

    bool atWarpSpeed()
    {
        return rend.velocityScale < warpDistortion;
    }

    bool atNormalSpeed()
    {
        return rend.velocityScale > 0;
    }

    void ChangeVelocity(Level difficulty)
    {
        switch (difficulty)
        {
            case Level.SuperEasy:
                particleSpeed = 0.2f;
                break;
            case Level.Easy:
                particleSpeed = 0.4f;
                break;
            case Level.Medium:
                particleSpeed = 0.6f;
                break;
            case Level.Hard:
                particleSpeed = 0.8f;
                break;
            case Level.God:
                particleSpeed = 1.5f;
                break;
            default:
                particleSpeed = 0.2f;
                Debug.LogWarning("<color=yellow> Received unexpected Difficulty level</color>");
                break;
        }
    }
}
