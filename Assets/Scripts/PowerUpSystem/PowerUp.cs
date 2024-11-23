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

        Vector3 screenMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 screenMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));

        if (transform.position.x < screenMin.x || transform.position.x > screenMax.x ||
            transform.position.y < screenMin.y || transform.position.y > screenMax.y)
        {
            PowerUpPoolManager.Instance.Return(_type, this);
        }
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
