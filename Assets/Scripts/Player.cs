using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int Score { get; set; }
    public int CorrectСlicks;


    public void ScoreReset()
    {
        Score = 0;
    }
    
}