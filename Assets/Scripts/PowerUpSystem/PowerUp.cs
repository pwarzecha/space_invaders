using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour, IPoolable
{
    [SerializeField] private PowerUpType _type;
    [SerializeField] private float _speed = 3.0f;
    private float minYPosition;
    public void OnCreated()
    {
        minYPosition = GameController.Instance.GameSettingsSO.screenBoundsY.x;

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

        if (transform.position.y < minYPosition)
            PowerUpPoolManager.Instance.Return(_type, this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PowerUpController powerUpController))
        {
            powerUpController.EnablePowerUp(_type);
            var vfx = VFXPoolManager.Instance.Get(VFXType.PowerUpPickup);
            vfx.transform.position = transform.position;
            PowerUpPoolManager.Instance.Return(_type, this);

        }
    }
}
