using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPassive : MonoBehaviour
{
    public UIPassive uiPassive;

    private Passive speedUp { get {  return uiPassive.speedUp; } }
    private Passive jumpUp { get { return uiPassive.jumpUp; } }

    private void Update()
    {
        if (speedUp.remainTime > 0f)
        {
            // CharacterManager.Instance.Player.controller.moveSpeed += 

            uiPassive.speedUp.uiBar.gameObject.SetActive(true);
            speedUp.remainTime -= Time.deltaTime;

            if (speedUp.remainTime == 0f)
            {
                uiPassive.speedUp.uiBar.gameObject.SetActive(false);
            }
        }
        
        if (jumpUp.remainTime > 0f)
        {
            uiPassive.jumpUp.uiBar.gameObject.SetActive(true);
            jumpUp.remainTime -= Time.deltaTime;

            if (jumpUp.remainTime == 0f)
            {
                uiPassive.jumpUp.uiBar.gameObject.SetActive(false);
            }
        }
    }
}
