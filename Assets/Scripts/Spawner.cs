using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoint;

    public SpawnData[] spawnData;

    float timer;
    int level;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();

    }

    void Update()
    {
        timer += Time.deltaTime;
        level = Mathf.Min( Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length-1 );
        
        if( timer > spawnData[level].spawnTime )
        {
            timer = 0f;
            Swapn();            
        }

    }

    void Swapn()
    {        
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1,spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

[System.Serializable]
public class SpawnData
{
    public int spriteType;

    public float spawnTime;        
    public int health;
    public float speed;
}

