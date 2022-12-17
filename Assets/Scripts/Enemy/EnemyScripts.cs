using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected SpriteRenderer sr;
    protected Animator anim;

    protected int _health;

    public int maxHeath;

    public int health
    {
        get => _health;
        set
        {
            _health = value;

            if (_health > maxHeath)
            {
                _health = maxHeath;
            }

            if (_health <= 0)
                Death();
        }
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        if (maxHeath <= 0)
            maxHeath = 10;

        health = maxHeath;
    }

    public virtual void Death()
    {
        anim.SetTrigger("Death");
    }    

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
    }
}