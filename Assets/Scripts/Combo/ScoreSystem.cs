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
    public Text comboDisp;

    public static int score;
    public static  int comboCount;

    public void Awake()
    {
        instRef = this;
    }

    // Increase score
    void UpdateScore()
    {
        score += scoreIncrement * comboCount;
        UpdateScoreUI();
    }

    // Increase combo count
    public void UpdateCombo()
    {
        Debug.Log("Combo");
        if(!comboDisp.gameObject.activeSelf)
        {
            comboDisp.gameObject.SetActive(true);
        }
        comboCount += 1;
        comboCount = comboCount > maxComboAllowed ? maxComboAllowed : comboCount;
        comboDisp.text = "" + comboCount + "X";
        UpdateScore();
    }

    // Reset combo count
    public void ComboBroken()
    {
        comboDisp.gameObject.SetActive(false);
        comboCount = 0;
        UpdateScoreUI();
    }

    // update Score Display
    void UpdateScoreUI()
    {
        scoreDisp.text="Score: "+score+" Pts.";
    }

    public int GetScore()
    {
        return score;
    }

}
