
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int StartingLives;
    public Transform SpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.lives = StartingLives;
        GameManager.instance.currentLevel = this;
        GameManager.instance.SpawnPlayer(SpawnPoint);
    }

    public void UpdateCheckpoint(Transform SpawnPoint)
    {
        GameManager.instance.currentSpawnPoint = SpawnPoint;
    }
}
