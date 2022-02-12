using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject resultPanelWin;
    [SerializeField] GameObject resultPanelLose;
    [SerializeField] Text resultText;
    [SerializeField] AudioSource audioSource;

    [SerializeField] Text playerHeroHpText;
    [SerializeField] Text enemyHeroHpText;

    [SerializeField] Text playerManaCostText;
    [SerializeField] Text enemyManaCostText;

    [SerializeField] Text timeCountText;

    public void HideResultPanel()
    {
        resultPanelWin.SetActive(false);
        resultPanelLose.SetActive(false);
    }

    public void ShowManaCost(int playerManaCost, int enemyManaCost)
    {
        playerManaCostText.text = playerManaCost.ToString();
        enemyManaCostText.text = enemyManaCost.ToString();
    }

    public void UpdateTime(int timeCount)
    {
        timeCountText.text = timeCount.ToString();

    }

    public void ShowHeroHp(int playerHeroHp, int enemyHeroHp)
    {
        playerHeroHpText.text = playerHeroHp.ToString();
        enemyHeroHpText.text = enemyHeroHp.ToString();

    }

    public void ShowResultPanel(int heroHp)
    {
        if (heroHp <= 0)
        {
            resultPanelLose.SetActive(true);
            resultText.text = "LOSE";
        }
        else
        {
            resultText.text = "WIN";
            resultPanelWin.SetActive(true);
        }
        audioSource.Stop();
    }

}
