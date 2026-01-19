using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private Player player;
    public static string PlayerName = "";
    

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
        Debug.Log(PlayerName);
        Highscores highscores = new Highscores();
        highscores.AddEntry(PlayerName, player.Score);
        SceneManager.LoadScene("GameOver");
        
    }
}
