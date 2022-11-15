using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pickups : MonoBehaviour
{
    public enum PickupType
    {
        Powerup = 0,
        Score = 2,
    }
    public PickupType currentPickup;

    private void OnTriggerEnter2D(Collider2D collision)
    {
    
        if (collision.gameObject.tag == "Player")
        {
            PlayerController curPlayer = collision.gameObject.GetComponent<PlayerController>();
            switch (currentPickup)
            {
                case PickupType.Powerup:
                    curPlayer.StartJumpForceChange();
                    break;
                case PickupType.Score:
                    Debug.Log("Score was picked up");
                    break;
            }
            
            Destroy(gameObject);
        }
    }
}
