using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIConditions : MonoBehaviour
{
    public Conditions health;
    public Conditions hunger;
    public Conditions stamina;

    private void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}
