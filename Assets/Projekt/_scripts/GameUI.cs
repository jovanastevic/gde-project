using System;
using UnityEngine;

public class GameUI : MonoBehaviour
{
   [SerializeField] private GameObject player;
   [SerializeField] private TMPro.TMP_Text scoreLabel;
   public void Update()
   {
      scoreLabel.text  = $"Score: {player.GetComponent<Player>().Score}";
   }
}
