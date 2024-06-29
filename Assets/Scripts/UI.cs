using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    [SerializeField]
    private TextMeshProUGUI _scoreText;
    private void Update()
    {
        if(!_scoreText)return;
        
        _scoreText.text = "Score: " + Convert.ToInt32(_player.currentScore * 10);
        
        
        
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    
    
    
}
