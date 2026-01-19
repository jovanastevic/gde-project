using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverLogic : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text ScoreLabel;
    [SerializeField] private TMPro.TMP_Text highscoreLable;
    
    public void Start()
    {
        ScoreLabel.text = $"Score: {Player.FinalScore}";
        Highscores highscores = new Highscores();
        highscoreLable.text = $"{highscores.ToString()}\n";
        
    }
    public void StartClicked()
    {
        Player.FinalScore = 0;
        SceneManager.LoadScene("GamePlay");
    }
    
    public void QuitButtonClicked()
    {
        Debug.Log("Quit button clicked");
        Application.Quit();
    }
}
