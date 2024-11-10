using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Text txtTotalEnemiesKilled;
    public int totalKills;
    public GameObject enemyContainer;

    void Start()
    {
        instance = this;
        totalKills = enemyContainer.GetComponentsInChildren<EnemyController>().Length;
        txtTotalEnemiesKilled.text = "Total Enemies: " + totalKills.ToString();
    }

    public void AddEnemyKill()
    {
        totalKills--;
        txtTotalEnemiesKilled.text = "Total Enemies: " + totalKills.ToString();
        if (totalKills <= 0)
        {
            FinGame(true);
        }
    }

    public void FinGame(bool isWin)
    {
        if (isWin)
        {
            Debug.Log("HAS GANADO");
            staticValues.winner = 1;
        }
        else
        {
            Debug.Log("HAS PERDIDO");
            staticValues.winner = 0;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
