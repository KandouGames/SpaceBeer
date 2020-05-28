using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicPool : MonoBehaviour
{
    //Not happy about this
    private GameObject bulletPrefab;

    public int numToCreate = 10;
    private Queue<GameObject> bulletQueue;
    private static DynamicPool _instance = null;
    private bool shouldExpand = true;

    public static DynamicPool instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject().AddComponent<DynamicPool>();
                _instance.name = "DynamicPool";

            }
            return _instance;
        }
    }

    public void GenerateBullets(GameObject bulletPrefab)
    {
        if (bulletQueue is null)
        {
            this.bulletPrefab = bulletPrefab;
            bulletQueue = new Queue<GameObject>();
            for (int i = 0; i < numToCreate; i++)
            {

                bulletQueue.Enqueue(CreateSingleBullet(bulletPrefab));
            }
        }
    }

    public GameObject GetBullet()
    {
        GameObject currentBullet;
        if (bulletQueue.Count != 0)
        {
            currentBullet = bulletQueue.Dequeue();
            currentBullet.SetActive(true);
        }
        else
        {
            currentBullet = CreateSingleBullet(bulletPrefab);
            currentBullet.SetActive(true);
        }
        return currentBullet;
    }

    public void ResetBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletQueue.Enqueue(bullet);
    }

    private GameObject CreateSingleBullet(GameObject bulletPrefab)
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.parent = this.transform;
        bullet.name = "bullet";
        bullet.SetActive(false);
        return bullet;
    }
}