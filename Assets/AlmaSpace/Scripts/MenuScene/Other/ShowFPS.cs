using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowFPS : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    private float prevFrameTime;
    private int framesCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Application.targetFrameRate = 60;
        prevFrameTime = Time.realtimeSinceStartup;
        framesCounter = 0;
    }


    // Update is called once per frame
    void Update()
    {
        float currentFrameTime = Time.deltaTime;
        int framesThisSecond = (int)(currentFrameTime / prevFrameTime);
        prevFrameTime = currentFrameTime;

        framesCounter += framesThisSecond;

        if (framesCounter >= 1)
        {
            int fps = (int)(framesThisSecond / currentFrameTime);
            textMeshPro.SetText(string.Format("FPS: " + fps.ToString()));
            framesCounter = 0;
        }

    }
}