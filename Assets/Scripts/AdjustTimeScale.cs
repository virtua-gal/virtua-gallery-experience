using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustTimeScale : MonoBehaviour
{
    public float timeScaleStartValue = 1;

    public float currentTimeScaleValue = 1;

    public float updateTimeScaleValue = 1;

    public float slowMoValue = 0.25f;

    public float slowMoResumeTime = 3;

    public float slowMoResumeValue = 1;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = timeScaleStartValue;
    }

    public void UpdateTimeScaleUI(string s)
    {
        float parsedValue = float.Parse(s);
        updateTimeScaleValue = parsedValue;
        Time.timeScale = updateTimeScaleValue;
    }

    public void ChangeTimeScale()
    {
        currentTimeScaleValue = updateTimeScaleValue;
        Time.timeScale = currentTimeScaleValue;
    }

    public void SlowMo()
    {
        Time.timeScale = slowMoValue;
        Invoke("ResetTimeScale", slowMoResumeTime * slowMoValue);
    }

    public void ResetTimeScale()
    {
        Time.timeScale = slowMoResumeValue;
    }
}
