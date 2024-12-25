using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapScript : MonoBehaviour
{
    [SerializeField] Transform Maps;
    [SerializeField] Transform CurrentMap;
    [SerializeField] GameObject FireworkEffect;
    [SerializeField] Transform FireworkPosParent;
    [SerializeField] GameObject TutorialText1;
    [SerializeField] GameObject TutorialText2;
    [SerializeField] TextScript TS;
    [SerializeField] GameObject LeftTimeParticle;

    public List<GameObject> targetStars = new List<GameObject>();
    public List<GameObject> targetBombs = new List<GameObject>();

    float waitingTime = 1000f;
    bool waiting = false;
    float fireworkTime = 0f;
    int fireworkNum = 0;


    public bool isTutorial = true;
    int tutorialMap = 0;

    public int currentMap = 0;

    float GameTime = 60f;
    public float lifeTime = -99999f;
    public int score = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        setToTutorial();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.SetInt("PlayerScore", -1);
            SceneManager.LoadScene("ShowScoreScene");
        }

        if (lifeTime >= 0)
        {
            lifeTime -= Time.deltaTime;
            ParticleSystem.ShapeModule shape = LeftTimeParticle.GetComponent<ParticleSystem>().shape;
            shape.arc = (lifeTime/GameTime) * 360f;
            //Debug.Log((lifeTime / GameTime) * 360f);
        }
        else if (lifeTime < 0 && -999 < lifeTime)
        {
            //The End!!
            //Debug.Log("THE END");
            lifeTime = -9999999f;
            //killAllObjs();
            //setToTutorial();
            PlayerPrefs.SetInt("PlayerScore", score);
            SceneManager.LoadScene("ShowScoreScene");
        }

        if (waiting && waitingTime < 2f)
        {
            waitingTime += Time.deltaTime;
            fireworkTime += Time.deltaTime;
            if(fireworkTime > 2f/3f)
            {
                fireworkTime = 0f;
                Vector3 pos = FireworkPosParent.GetChild(fireworkNum).position;
                Instantiate(FireworkEffect, pos, FireworkEffect.transform.rotation);
                fireworkNum += 1;
            }
        }
        else if(waiting)
        {
            waiting = false;
            waitingTime = 1000f;

            if(isTutorial)
            {
                tutorialMap += 1;
                if(tutorialMap == 5 + 1)
                {
                    offTutorial();
                }
                else
                {
                    selectMap(tutorialMap);
                }
            }
            else
            {
                if(currentMap == 0)
                {
                    //처음 게임 시작
                    lifeTime = GameTime;
                    score = 0;
                    selectMap(Random.Range(1, 3 + 1));
                }
                else
                {
                    selectMap(Random.Range(1, 6 + 1));
                }
            }
        }

        if(targetStars.Count == 0 && !waiting)
        {
            resetMap();
        }
    }

    void killAllObjs()
    {
        for (int i = 0; i < targetStars.Count; i++)
        {
            Destroy(targetStars[i].gameObject);
        }
        for (int i = 0; i < targetBombs.Count; i++)
        {
            Destroy(targetBombs[i].gameObject);
        }
        targetStars = new List<GameObject>();
        targetBombs = new List<GameObject>();

        for (int i = 0; i < CurrentMap.childCount; i++)
        {
            Destroy(CurrentMap.GetChild(i).gameObject);
        }
    }

    void resetMap()
    {
        killAllObjs();

        waitingTime = 0f;
        waiting = true;
        fireworkTime = 0.2f;
        fireworkNum = 0;
    }

    void setToTutorial()
    {
        score = 0;
        TutorialText1.SetActive(true);
        TutorialText2.SetActive(true);
        LeftTimeParticle.SetActive(false);
        isTutorial = true;
        tutorialMap = 0;

        selectMap(0);
    }

    void offTutorial()
    {
        score = 0;
        TutorialText1.SetActive(false);
        TutorialText2.SetActive(false);
        LeftTimeParticle.SetActive(true);
        isTutorial = false;
        tutorialMap = 0;

        selectMap(0);
    }

    void selectMap(int n)
    {
        TS.showWriteAnimation(n);
        currentMap = n;

        Transform targetMap = Maps.GetChild(n);
        Transform clonedMap = Instantiate(targetMap, CurrentMap);
        clonedMap.gameObject.SetActive(true);
        for (int i = 0; i < clonedMap.childCount; i++)
        {
            GameObject targetObject = clonedMap.GetChild(i).gameObject;
            if (targetObject.CompareTag("Star"))
            {
                targetStars.Add(targetObject);
            }
            else if (targetObject.CompareTag("Bomb"))
            {
                targetBombs.Add(targetObject);
            }
        }
    }
}
