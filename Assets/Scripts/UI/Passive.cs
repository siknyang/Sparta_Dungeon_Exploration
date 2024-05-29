using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Passive : MonoBehaviour
{
    public float remainTime;
    public float maxTime;
    public float buffTime;
    public Image uiBar;

    private void Start()
    {
        remainTime = maxTime;
    }

    private void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    private float GetPercentage()
    {
        return remainTime / maxTime;
    }

    public void AddBuff(float value)
    {
        remainTime += value;
    }
}
