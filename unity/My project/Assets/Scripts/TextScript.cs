using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextScript : MonoBehaviour
{
    [SerializeField] GameObject P1Title;
    [SerializeField] GameObject P2Title;
    [SerializeField] GameObject P1Subtitle;
    [SerializeField] GameObject P2Subtitle;

    [SerializeField] List<string> titleTexts = new List<string>();
    [SerializeField] List<string> subtitleTexts = new List<string>();

    string targetTitle = "";
    string targetSubitle = "";
    float showTextTime = 0f;

    private void Start()
    {
        showWriteAnimation(0);
    }

    private void Update()
    {
        if(showTextTime < 1f)
        {
            string titleCut = targetTitle.Substring(0, (int)(targetTitle.Length * showTextTime));
            string subtitleCut = targetSubitle.Substring(0, (int)(targetSubitle.Length * showTextTime));

            P1Title.GetComponent<TMPro.TextMeshPro>().text = titleCut;
            P2Title.GetComponent<TMPro.TextMeshPro>().text = titleCut;

            P1Subtitle.GetComponent<TMPro.TextMeshPro>().text = subtitleCut;
            P2Subtitle.GetComponent<TMPro.TextMeshPro>().text = subtitleCut;
            showTextTime += Time.deltaTime;
        }
        else
        {
            P1Title.GetComponent<TMPro.TextMeshPro>().text = targetTitle;
            P2Title.GetComponent<TMPro.TextMeshPro>().text = targetTitle;

            P1Subtitle.GetComponent<TMPro.TextMeshPro>().text = targetSubitle;
            P2Subtitle.GetComponent<TMPro.TextMeshPro>().text = targetSubitle;
        }
    }

    public void showWriteAnimation(int i)
    {
        targetTitle = titleTexts[i];
        targetSubitle = subtitleTexts[i];
        showTextTime = 0f;
    }
}
