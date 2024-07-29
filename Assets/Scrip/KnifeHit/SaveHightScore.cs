using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveHightScore : MonoBehaviour
{
    public TextMeshProUGUI _hightScore;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("HightScore"))
        {
            _hightScore.text = "Hight Scores : "+PlayerPrefs.GetInt("HightScore").ToString();
        }
        else
        {
            int hightScore = 0;
            PlayerPrefs.SetInt("HightScore", hightScore);
            _hightScore.text = "Hight Scores : "+PlayerPrefs.GetInt("HightScore").ToString();
        }
        PlayerPrefs.Save();
    }

    public void UpdateHightScore(int hightScore)
    {
        PlayerPrefs.SetInt("HightScore", hightScore);
        PlayerPrefs.Save();
    }
    public int GetHightScore()
    {
        return PlayerPrefs.GetInt("HightScore");
    }
}
