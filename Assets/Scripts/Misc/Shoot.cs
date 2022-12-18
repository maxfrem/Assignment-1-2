using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Shoot : MonoBehaviour
{
    SpriteRenderer sr;
     AudioSourceManager asm;
    //AudioSourceManager asm;

    public float projectileSpeed;
    public Transform SpawnPointRight;
    public Transform SpawnPointLeft;

    public Projectile projectilePrefab;

    public UnityEvent OnProjectileSpawned;


    public AudioClip FireSound;
    //Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (projectileSpeed <= 0)
            projectileSpeed = 7.0f;

        if (!SpawnPointLeft || !SpawnPointRight || !projectilePrefab)
            Debug.Log("Please setup default values on: " + gameObject.name);
    }

    public void Fire()
    {
        if (!sr.flipX)
        {
            Projectile curProjectile = Instantiate(projectilePrefab, SpawnPointRight.position, SpawnPointRight.rotation);
            curProjectile.speed = projectileSpeed;
        }
        else
        {
            Projectile curProjectile = Instantiate(projectilePrefab, SpawnPointLeft.position, SpawnPointLeft.rotation);
            curProjectile.speed = -projectileSpeed;
        }

        if (asm)
            asm.PlayOneShot(FireSound, false);

        OnProjectileSpawned?.Invoke();

    }
}