using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAnimation : MonoBehaviour
{
    [SerializeField] float RotationSpeed = 10f;

    float waitTime = 0f;
    float sizeSin = 0f;
    float x = 0;
    float y = 0;

    void Update()
    {
        if(waitTime > 0.5f)
        {
            if (x > 0.48f)
            {
                this.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                //Destroy(this.gameObject.GetComponent<ShowAnimation>());
            }
            else
            {
                x = Mathf.Lerp(x, 0.5f, Time.deltaTime * 2f);
                this.transform.localScale = new Vector3(x, x, x);
            }

            y = Time.deltaTime * RotationSpeed;
            this.transform.Rotate(new Vector3(y * 0.5f, y * 1.2f, y));

            sizeSin += Time.deltaTime * 2f;
            if(sizeSin > Mathf.PI * 2)
            {
                sizeSin -= Mathf.PI * 2;
            }
            float tmp = Mathf.Sin(sizeSin) * 0.07f;
            this.transform.localScale = new Vector3(0.5f + tmp, 0.5f + tmp, 0.5f + tmp);
        }
        else
        {
            waitTime += Time.deltaTime;
        }
    }
}
