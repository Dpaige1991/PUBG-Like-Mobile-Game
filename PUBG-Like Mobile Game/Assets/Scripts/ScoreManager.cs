using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("Score Manager")]
    public int kills;
    public int enemyKills;
    public TMP_Text playerKillCounter;
    public TMP_Text enemyKillCounter;
    public TMP_Text mainText;

    private void Awake()
    {
        if(PlayerPrefs.HasKey("kills"))
        {
            kills = PlayerPrefs.GetInt("0");
        }
        else if (PlayerPrefs.HasKey("enemyKills"))
        {
            enemyKills = PlayerPrefs.GetInt("0");
        }
    }

    void Update()
    {
        StartCoroutine(WinOrLose());
    }

    IEnumerator WinOrLose()
    {
        playerKillCounter.text = "" + kills;
        enemyKillCounter.text = "" + enemyKills;

        if(kills >= 10)
        {
            mainText.text = "Blue Team Victory";
            PlayerPrefs.SetInt("kills", kills);
            Time.timeScale = 0f;
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("TDMRoom");
        }
        else if (kills >= 10)
        {
            mainText.text = "Red Team Victory";
            PlayerPrefs.SetInt("enemykills", kills);
            Time.timeScale = 0f;
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("TDMRoom");
        }
    }
}
