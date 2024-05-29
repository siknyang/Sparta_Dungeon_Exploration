using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void TakePhysicalDamage(int damage);
}

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UIConditions uiCondition;

    private Conditions health { get { return uiCondition.health; } }
    private Conditions hunger { get { return uiCondition.hunger; } }
    private Conditions stamina { get { return uiCondition.stamina; } }

    public float noHungerHealthDecay;

    public event Action onTakeDamage;

    private void Update()
    {
        // 시간이 지날 때마다 배고픔 깎이고 스태미나 채워지고
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        // 만약 배고픔이 0이면 체력이 깎임
        if (hunger.curValue == 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        // 만약 체력이 0이면 사망
        if (health.curValue == 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    private void Die()
    {
        Debug.Log("사망");
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }
}
