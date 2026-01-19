using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogicStartMenu : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField nameField;

    public void StartClicked()
    {
        string playerName = nameField.text;
        GameLogic gemeGameLogic = new GameLogic();
        gemeGameLogic.PlayerName = playerName;
        Debug.Log(playerName);
        SceneManager.LoadScene("GamePlay");
    }

    public void QuitButtonClicked()
    {
        Debug.Log("Quit button clicked");
        Application.Quit();
    }
}
