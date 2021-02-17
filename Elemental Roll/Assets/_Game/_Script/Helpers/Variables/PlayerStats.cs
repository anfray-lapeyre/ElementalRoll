using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats 
{
    public int activePlayer { get; set; }
    public int jumpCount { get; set; }
    public int currentLevel { get; set; }
    public int DifficultyLife { get; set; }
    public int DifficultyChrono { get; set; }
    public float InGameUI_Opacity { get; set; }
    public int livesLeft { get; set; }
    public float[][] maxPowerTime { get; set; }
    public float playerRotationSpeed { get; set; }
    public float playerSpeed { get; set; }
    public float[][] powerTime { get; set; }
    public float Time { get; set; }
    public float TimePlayed { get; set; }

    public PlayerStats(int _jumpCount, int _currentLevel, int _DifficultyLife, int _DifficultyChrono, float _InGameUI_Opacity, int _livesLeft, float[][] _maxPowerTime, float _playerRotationSpeed, float _playerSpeed,  float[][] _powerTime, float _Time, float _TimePlayed)
    {
        activePlayer = 0;
        jumpCount = _jumpCount;
        currentLevel = _currentLevel;
        DifficultyLife = _DifficultyLife;
        DifficultyChrono = _DifficultyChrono;
        InGameUI_Opacity = _InGameUI_Opacity;
        livesLeft = _livesLeft;
        maxPowerTime = (float[][])_maxPowerTime.Clone();
        playerRotationSpeed = _playerRotationSpeed;
        playerSpeed = _playerSpeed;
        powerTime = (float[][])_powerTime.Clone();
        Time = _Time;
        TimePlayed = _TimePlayed;
    }

    public PlayerStats()
    {
        activePlayer = 0;
        jumpCount = 0;
        currentLevel = 0;
        DifficultyLife = 0;
        DifficultyChrono = 0;
        InGameUI_Opacity = 0;
        livesLeft = 0;

        maxPowerTime = new float[4][];
        for(int i = 0; i < maxPowerTime.Length; i++)
        {
            maxPowerTime[i] = new float[2];
            maxPowerTime[i][0] = Character.getCharacterInfo(i).powerTime;
            maxPowerTime[i][1] = Character.getCharacterInfo(i).secondPowerTime;
        }

        playerRotationSpeed = 0;
        playerSpeed = 0;
        powerTime = new float[4][];
        for (int i = 0; i < powerTime.Length; i++)
        {
            powerTime[i] = new float[2];
            powerTime[i][0] = Character.getCharacterInfo(i).powerTime;
            powerTime[i][1] = Character.getCharacterInfo(i).secondPowerTime;
        }
        Time = 0;
        TimePlayed = 0;
    }

}
