using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("#Game Control")]
    public bool isLive;
    public float gameTime;    
    public float maxGameTime;

    [Header("#Player Info")]
    public int playerId;
    public int level;
    public int Kill;
    public int exp;
    public int[] nextExp = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };
    public float health;
    public float maxHealth;

    [Header("#Game Object")]
    public Player player;
    public PoolManager pool;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject enemyCleaner;

    private void Awake()
    {
        instance = this;        
    }
    public void GameStart(int id)
    {
        playerId = id;
               

        health = maxHealth;
        player.gameObject.SetActive(true);
        uiLevelUp.Select(playerId%2);
        isLive = true;
        
        Resume();
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;
        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();        
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutin());
    }

    IEnumerator GameVictoryRoutin()
    {
        isLive = false;
        enemyCleaner.SetActive(true);        
        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(0);
    }

    void Update()
    {
        if (!isLive) { return; }

        gameTime += Time.deltaTime;
                
        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();
        }
    }

    public void GetExp()
    {
        if (!isLive) { return; }
        exp++;

        if( exp == nextExp[Mathf.Min(level,nextExp.Length-1)] )
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }
}
