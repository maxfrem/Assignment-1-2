using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : Enemy
{
    public float projectileFireRate;
    public float turretFireDistance;
    float timeSinceLastFire;
    Shoot shootScript;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        shootScript = GetComponent<Shoot>();
        shootScript.OnProjectileSpawned.AddListener(UpdateTimeSinceLastFire);

    }

    private void OnDisable()
    {
        shootScript.OnProjectileSpawned.RemoveListener(UpdateTimeSinceLastFire);
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorClipInfo[] currentClips = anim.GetCurrentAnimatorClipInfo(0);

        if (currentClips[0].clip.name != "Fire")
        {
            if (GameManager.instance.playerInstance)
            {
                sr.flipX = (GameManager.instance.playerInstance.transform.position.x < transform.position.x) ? true : false;
            }

            float distance = Vector2.Distance(GameManager.instance.playerInstance.transform.position, transform.position);
            
            sr.color = (distance <= turretFireDistance) ? Color.red : Color.white;

            if (distance <= turretFireDistance && Time.time >= timeSinceLastFire + projectileFireRate)
            {
                 anim.SetTrigger("Fire");
            }
        }
    }   
    

    public override void Death()
    {
        Destroy(gameObject);
    }

    void UpdateTimeSinceLastFire()
    {
        timeSinceLastFire = Time.time;
    }
}