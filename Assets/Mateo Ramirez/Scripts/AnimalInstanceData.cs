
using System;
using UnityEngine;

[Serializable]
public class AnimalInstanceData
{
    [Header("Identificación")]
    [SerializeField] private string instanceId;

    [Header("Estado individual")]
    [SerializeField] private bool isSick;
    [SerializeField] private bool isAlive = true;
    [SerializeField, Min(0f)] private float health = 100f;

    public string InstanceId => instanceId;
    public bool IsSick => isSick;
    public bool IsAlive => isAlive;
    public float Health => health;

    public AnimalInstanceData()
    {
        instanceId = Guid.NewGuid().ToString();
    }

    public void SetSick(bool value)
    {
        isSick = value;
    }

    public void SetAlive(bool value)
    {
        isAlive = value;
    }

    public void SetHealth(float value)
    {
        health = Mathf.Max(0f, value);

        if (health <= 0f)
        {
            isAlive = false;
        }
    }
}

