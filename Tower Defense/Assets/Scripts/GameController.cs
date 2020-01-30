using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int life, coins;
    private int score, maxScoreLength;

    public Text scoreTXT, lifeTXT, coinsTXT, finalScoreTXT;

    public Animator lifeANIM;

    public Sprite lifeSprite;
    public SpriteRenderer lifeSR;

    void Start()
    {
        score = 0;
        maxScoreLength = 7;//MAX SCORE TO BE APPEARED WHEN LEVEL SUCCEED IS 9.999.999

        scoreTXT.text = score.ToString();
        lifeTXT.text = life.ToString();
        coinsTXT.text = coins.ToString();
    }

    public void AddScore(int i)
    {
        score += i;
        string scoreString = score.ToString();
        scoreTXT.text = scoreString;

        if (scoreString.Length > maxScoreLength)
            finalScoreTXT.text = "9999999";
        else
        {
            finalScoreTXT.text = scoreString;
        }
    }

    public void AddCoins(int i)
    {
        coins += i;
        coinsTXT.text = coins.ToString();
    }
    public void LoseCoins(int i)
    {
        coins -= i;
        coinsTXT.text = coins.ToString();
    }

    public void LoseLife()
    {
        life--;
        if (life > 0)
        {
            lifeANIM.enabled = true;
            Invoke("StopAnim", lifeANIM.GetCurrentAnimatorClipInfo(0).Length);
            lifeTXT.text = life.ToString();
        }
        else
            SceneManager.LoadScene("GameOver");
    }
    public void StopAnim()
    {
        lifeANIM.enabled = false;
        lifeSR.sprite = lifeSprite;
    }
}
