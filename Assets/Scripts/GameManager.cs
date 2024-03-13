using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("#Game Control")]
    public float gameTime;    
    public float maxGameTime;

    [Header("#Player Info")]
    public int level;
    public int Kill;
    public int exp;
    public int[] nextExp = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };
    public int health;
    public int maxHealth;

    [Header("#Game Object")]
    public Player player;
    public PoolManager pool;


    private void Awake()
    {
        instance = this;        
    }
    private void Start()
    {
        maxGameTime = 2 * 10f;
        health = maxHealth;
    }

    void Update()
    {
        gameTime += Time.deltaTime;
                
        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            
        }
    }

    public void GetExp()
    {
        exp++;

        if( exp == nextExp[level] )
        {
            level++;
            exp = 0;
        }
    }
}
