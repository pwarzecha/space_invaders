using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour, IPoolable
{
    [SerializeField] private PowerUpType _type;
    [SerializeField] private float _speed = 3.0f;

    public void OnCreated()
    {

    }

    public void OnPooled()
    {

    }

    public void OnReturn()
    {

    }

    private void Update()
    {
        transform.position += Vector3.down * (_speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PowerUpController powerUpController))
        {
            powerUpController.EnablePowerUp(_type);
            PowerUpPoolManager.Instance.Return(_type, this);
        }
    }
}
