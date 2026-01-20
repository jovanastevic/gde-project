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
        Player.PlayerWon += WinScreen;
    }

    private void OnDisable()
    {
        Player.PlayerIsDead -= GameOver;
        Player.PlayerWon -= WinScreen;
    }

    private void WinScreen()
    {
        Highscores highscores = new Highscores();
        highscores.AddEntry(PlayerName, player.Score);
        SceneManager.LoadScene("WinScreen");
    }
    
    private void GameOver()
    {
        Highscores highscores = new Highscores();
        highscores.AddEntry(PlayerName, player.Score);
        SceneManager.LoadScene("GameOver");
        
    }
}
