using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PowerUpBehaviourData
{
    public PowerUpType PowerUpType;
    [SerializeField]
    private MonoBehaviour _behaviour;
    public IPowerUpBehaviour PowerUpBehaviour => _behaviour as IPowerUpBehaviour;
}
public interface IPowerUpBehaviour
{
    void EnablePowerUp();
    void DisablePowerUp();
}
public class PowerUpController : MonoBehaviour
{
    [SerializeField] private List<PowerUpBehaviourData> _behaviourData;

    public void EnablePowerUp(PowerUpType powerUpType)
    {
        foreach (var data in _behaviourData)
        {
            if (data.PowerUpType == powerUpType && data.PowerUpBehaviour != null)
            {
                data.PowerUpBehaviour.EnablePowerUp();
                return;
            }
        }
    }

    public void DisableAllPowerUps()
    {
        foreach (var data in _behaviourData)
        {
            if (data.PowerUpBehaviour != null)
                data.PowerUpBehaviour.DisablePowerUp();
        }
    }
}
