// using UnityEngine;
//
// public class StartGameMenu : MonoBehaviour
// {
//     [SerializeField] private GameLogic gameLogic;
//     [SerializeField] private TMPro.TMP_InputField nameField;
//     void OnEnable()
//     {
//         TimeManager.SetPause(gameObject);
//     }
//
//     void OnDisable()
//     {
//         TimeManager.RemovePause(gameObject);
//     }
//     public void StartClicked()
//     {
//         string playerName = nameField.text;
//         gameLogic.PlayerName = playerName;
//         gameObject.SetActive(false);
//     }
// }