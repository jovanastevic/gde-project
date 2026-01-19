using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private Player player;
    public string PlayerName = "";
    

    private void OnEnable()
    {
        Player.PlayerIsDead += GameOver;
    }

    private void OnDisable()
    {
        Player.PlayerIsDead -= GameOver;
    }
    
    private void GameOver()
    {
        SceneManager.LoadScene("GameOver");
        
    }
}
