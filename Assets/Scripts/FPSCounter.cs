using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] Text textFps;
    int fpsCounter = 0;
    float mTimeCounter = 0.0f;
    float lastFrameRate = 0.0f;
    public float refrash = 0.5f;
    const string format = "{0} fps";

    // Update is called once per frame
    void Update()
    {
        if (mTimeCounter < refrash)
        {
            fpsCounter++;
            mTimeCounter += Time.deltaTime;

        }
        else
        {
            lastFrameRate = fpsCounter / mTimeCounter;
            mTimeCounter = 0.0f;
            fpsCounter = 0;
        }
        textFps.text = string.Format(format, (int)lastFrameRate);
    }
}
