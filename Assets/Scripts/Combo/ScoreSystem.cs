/**
Script Author : Vaikash 
Description   : Score Manager - Jump and Combo
**/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{

    public static ScoreSystem instRef;

    public int scoreIncrement;
    public int maxComboAllowed;

    // UI 
    public Text scoreDisp;

    int score;
    int comboCount;

    public void Awake()
    {
        instRef = this;
    }

    // Increase score
    void UpdateScore()
    {
        score += scoreIncrement * comboCount;
    }

    // Increase combo count
    public void UpdateCombo()
    {
        comboCount += 1;
        comboCount = comboCount > maxComboAllowed ? maxComboAllowed : comboCount;
        UpdateScore();
    }

    // Reset combo count
    public void ComboBroken()
    {
        comboCount = 0;
    }

    // update Score Display
    void UpdateScoreUI()
    {

    }

}
