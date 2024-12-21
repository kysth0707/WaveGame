using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowScoreScript : MonoBehaviour
{

    [SerializeField] GameObject ScoreText;


    float waiting = 0f;
    void Start()
    {
        int score = PlayerPrefs.GetInt("PlayerScore");
        ScoreText.GetComponent<TMPro.TextMeshPro>().text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        waiting += Time.deltaTime;
        if(waiting > 5f)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
