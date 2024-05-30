using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Passive : MonoBehaviour
{
    public float remainTime;
    public float buffTime;
    public Image uiBar;
    public Image uiBG;

    private void Start()
    {
        remainTime = buffTime;
    }

    private void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    private float GetPercentage()
    {
        return remainTime / buffTime;
    }

    public void AddBuff(float value)
    {
        remainTime += value;
        // UI도 업데이트 되어야 함
    }
}
