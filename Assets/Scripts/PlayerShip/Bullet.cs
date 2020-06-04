using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    IEnumerator startDeathTimer;
    public void Shoot(float bulletSpeed, float lifeTime)
    {
        GetComponent<Rigidbody>().AddForce(Vector3.forward * bulletSpeed);
        startDeathTimer = Deactivate(lifeTime);
        StartCoroutine(startDeathTimer);
    }

    IEnumerator Deactivate(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);

        GetComponentInChildren<ParticleSystem>()?.Clear(true);

        DynamicPool.instance.ResetObj(this.gameObject, DynamicPool.objType.Bullet);
    }
}
