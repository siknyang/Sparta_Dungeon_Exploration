using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPassive : MonoBehaviour
{
    public UIPassive uiPassive;
    public Player player;

    private Passive speedUp { get {  return uiPassive.speedUp; } }
    private Passive jumpUp { get { return uiPassive.jumpUp; } }

    //private void Update()
    //{
    //    if (speedUp.remainTime > 0f)
    //    {
    //        uiPassive.speedUp.uiBar.gameObject.SetActive(true);
            

    //        if (speedUp.remainTime == 0f)
    //        {
    //            uiPassive.speedUp.uiBar.gameObject.SetActive(false);
    //        }
    //    }

    //    if (jumpUp.remainTime > 0f)
    //    {
    //        uiPassive.jumpUp.uiBar.gameObject.SetActive(true);
    //        jumpUp.remainTime -= Time.deltaTime;

    //        if (jumpUp.remainTime == 0f)
    //        {
    //            uiPassive.jumpUp.uiBar.gameObject.SetActive(false);
    //        }
    //    }
    //}

    public void SpeedUpBuff(float time, float buffSpeed)
    {
        //speedUp.AddBuff(value);
        StartCoroutine(ApplySpeedBuff(time, buffSpeed));
    }

    private IEnumerator ApplySpeedBuff(float time, float buffSpeed)
    {
        float curSpeed = CharacterManager.Instance.Player.controller.moveSpeed;
        CharacterManager.Instance.Player.controller.moveSpeed += buffSpeed;
        uiPassive.speedUp.uiBG.gameObject.SetActive(true);

        for (float t = 0f; t < time; t += Time.deltaTime)
        {
            speedUp.uiBar.fillAmount = ( time - t ) / time;
            yield return null;
        }

        CharacterManager.Instance.Player.controller.moveSpeed = curSpeed;
        speedUp.uiBar.fillAmount = 0;
        uiPassive.speedUp.uiBG.gameObject.SetActive(false);
    }

    public void JumpUpBuff(float time, float buffPower)
    {
        //jumpUp.AddBuff(value);
        StartCoroutine(ApplyJumpUpBuff(time, buffPower));
    }

    private IEnumerator ApplyJumpUpBuff(float time, float buffPower)
    {
        float curPower = CharacterManager.Instance.Player.controller.jumpPower;
        CharacterManager.Instance.Player.controller.jumpPower += buffPower;
        uiPassive.jumpUp.uiBG.gameObject.SetActive(true);

        for (float t = 0f; t < time; t += Time.deltaTime)
        {
            jumpUp.uiBar.fillAmount = (time - t) / time;
            yield return null;
        }

        CharacterManager.Instance.Player.controller.jumpPower = curPower;
        jumpUp.uiBar.fillAmount = 0;
        uiPassive.jumpUp.uiBG.gameObject.SetActive(false);
    }
}
