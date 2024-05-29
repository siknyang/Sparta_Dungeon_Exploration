using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPassive : MonoBehaviour
{
    public Passive speedUp;
    public Passive jumpUp;

    void Start()
    {
        CharacterManager.Instance.Player.passive.uiPassive = this;
    }
}