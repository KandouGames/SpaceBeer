using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public ParticleSystem fire;
    public ParticleSystem smoke;
    public ParticleSystem sparks;

    public void PlaySmokeExplosion()
    {
        smoke.Play();
        sparks.Play();
    }

    public void PlayFullExplosion()
    {
        fire.Play();
        smoke.Play();
        sparks.Play();
    }
}
