using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveScript : MonoBehaviour
{
    public LineRenderer WaveLine;
    [SerializeField] MapScript mapScript;
    [SerializeField] GameObject starSuccessEffect;
    [SerializeField] GameObject bombSuccessEffect;

    float[] wavePoses = new float[81];
    // 위에는 세트로 수정하기

    [SerializeField] float waveMoveWait = 0.05f;

    [SerializeField] float[] sinWave = new float[11];

    [SerializeField] bool P1_UpWave = false;
    [SerializeField] bool P1_DownWave = false;
    [SerializeField] bool P2_UpWave = false;
    [SerializeField] bool P2_DownWave = false;
    [SerializeField] bool P12_UpWave = false;
    [SerializeField] bool P12_DownWave = false;
    [SerializeField] bool P12_NoWave = false;

    List<int> P1Wave_Direction = new List<int>();
    List<int> P1Wave_Pos = new List<int>();
    List<int> P2Wave_Direction = new List<int>();
    List<int> P2Wave_Pos = new List<int>();

    float lastTime = -1f;

    void Start()
    {
        for(int i = 0; i < wavePoses.Length; i++)
        {
            wavePoses[i] = 0f;
        }
        for (int i = 0; i < sinWave.Length; i++)
        {
            sinWave[i] *= 1.5f;
        }

        WaveLine.positionCount = wavePoses.Length;
        for (int i = 0; i < wavePoses.Length; i++)
        {
            WaveLine.SetPosition(i, new Vector3(-7 + (14f/(wavePoses.Length - 1))*i, 0, 0));
            //if (i < 11)
            //{
            //    WaveLine.SetPosition(i, new Vector3(-7 + (14f / 100) * i, 0, sinWave[i]));
            //}
        }
    }

    void Update()
    {
        testByButtons();

        moveWave();
        drawWave();
        showWave();
        checkWaveWithCollider();
    }

    void checkWaveWithCollider()
    {
        //mapScript.targetStars
        //mapScript.targetBombs

        for(int i = 0; i < wavePoses.Length; i++)
        {
            Vector3 targetPos = new Vector3(-7 + (14f / (wavePoses.Length - 1)) * i, 0, wavePoses[i]);
            for (int x = 0; x < mapScript.targetStars.Count; x++)
            {
                if (mapScript.targetStars[x].transform.GetChild(0).GetComponent<BoxCollider>().bounds.Contains(targetPos))
                {
                    GameObject tmp = mapScript.targetStars[x];
                    mapScript.targetStars.RemoveAt(x);

                    Instantiate(starSuccessEffect, tmp.transform.position + new Vector3(0, 1, 0), starSuccessEffect.transform.rotation);
                    Destroy(tmp);

                    x -= 1;

                    if(!mapScript.isTutorial)
                    {
                        mapScript.score += 1;
                    }
                }
            }

            for (int x = 0; x < mapScript.targetBombs.Count; x++)
            {
                if (mapScript.targetBombs[x].transform.GetChild(0).GetComponent<BoxCollider>().bounds.Contains(targetPos))
                {
                    GameObject tmp = mapScript.targetBombs[x];
                    mapScript.targetBombs.RemoveAt(x);

                    Instantiate(bombSuccessEffect, tmp.transform.position + new Vector3(0, 1, 0), bombSuccessEffect.transform.rotation);
                    Destroy(tmp);

                    x -= 1;

                    if (!mapScript.isTutorial)
                    {
                        mapScript.lifeTime -= 3;
                    }
                }
            }
        }
    }

    void moveWave()
    {
        if((Time.time - lastTime) < waveMoveWait)
        {
            return;
        }
        lastTime = Time.time;

        for (int i = 0; i < P1Wave_Pos.Count; i++)
        {
            P1Wave_Pos[i] += 1;
        }
        for (int i = 0; i < P2Wave_Pos.Count; i++)
        {
            P2Wave_Pos[i] -= 1;
        }
        if (P1Wave_Pos.Count > 0)
        {
            if (P1Wave_Pos[0] == wavePoses.Length + sinWave.Length / 2)
            {
                P1Wave_Pos.RemoveAt(0);
                P1Wave_Direction.RemoveAt(0);
            }
        }

        if (P2Wave_Pos.Count > 0)
        {
            if (P2Wave_Pos[0] == -sinWave.Length / 2)
            {
                P2Wave_Pos.RemoveAt(0);
                P2Wave_Direction.RemoveAt(0);
            }
        }
    }

    void showWave()
    {
        for (int i = 0; i < wavePoses.Length; i++)
        {
            WaveLine.SetPosition(i, new Vector3(-7 + (14f / (wavePoses.Length - 1)) * i, 0, wavePoses[i]));
        }
    }

    void drawWave()
    {
        for (int i = 0; i < wavePoses.Length; i++)
        {
            wavePoses[i] = 0f;
        }

        for (int i = 0; i < P1Wave_Pos.Count; i++)
        {
            int waveCenter = P1Wave_Pos[i];
            int dir = P1Wave_Direction[i];

            int x2 = 0;
            for (int x = -(sinWave.Length - 1) / 2; x <= (sinWave.Length - 1) / 2; x++)
            {
                if (0 <= waveCenter + x && waveCenter + x < wavePoses.Length)
                {
                    wavePoses[waveCenter + x] += sinWave[x2] * dir;
                }
                x2++;
            }
        }

        for (int i = 0; i < P2Wave_Pos.Count; i++)
        {
            int waveCenter = P2Wave_Pos[i];
            int dir = P2Wave_Direction[i];

            int x2 = 0;
            for (int x = -(sinWave.Length - 1) / 2; x <= (sinWave.Length - 1) / 2; x++)
            {
                if (0 <= waveCenter + x && waveCenter + x < wavePoses.Length)
                {
                    wavePoses[waveCenter + x] += sinWave[x2] * dir;
                }
                x2++;
            }
        }
    }

    void testByButtons()
    {
        if (P1_UpWave)
        {
            P1_UpWave = false;
            addP1WaveUp();
        }

        if (P1_DownWave)
        {
            P1_DownWave = false;
            addP1WaveDown();
        }

        if (P2_UpWave)
        {
            P2_UpWave = false;
            addP2WaveUp();
        }

        if (P2_DownWave)
        {
            P2_DownWave = false;
            addP2WaveDown();
        }

        if (P12_UpWave)
        {
            P12_UpWave = false;
            addP1WaveUp();
            addP2WaveUp();
        }

        if (P12_DownWave)
        {
            P12_DownWave = false;
            addP1WaveDown();
            addP2WaveDown();
        }

        if (P12_NoWave)
        {
            P12_NoWave = false;
            addP1WaveUp();
            addP2WaveDown();
        }
    }

    public void addP1WaveUp()
    {
        P1Wave_Pos.Add(-sinWave.Length / 2);
        P1Wave_Direction.Add(1);
    }

    public void addP1WaveDown()
    {
        P1Wave_Pos.Add(-sinWave.Length / 2);
        P1Wave_Direction.Add(-1);
    }
    public void addP2WaveUp()
    {
        P2Wave_Pos.Add(wavePoses.Length - 1 + sinWave.Length / 2);
        P2Wave_Direction.Add(1);
    }

    public void addP2WaveDown()
    {
        P2Wave_Pos.Add(wavePoses.Length - 1 + sinWave.Length / 2);
        P2Wave_Direction.Add(-1);
    }
}
