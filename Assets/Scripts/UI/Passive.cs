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

    // TODO :: 동일한 패시브 아이템 사용했을 때 시간 증가 안 되는 버그 수정
    // 20이 증가된 상태로 20초가 아니라, 20이 두 번 증가해서 40 증가함
}