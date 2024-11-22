using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAmountPowerUpBehaviour : MonoBehaviour, IPowerUpBehaviour
{
    [SerializeField] Player _player;
    [SerializeField] private int _projectilesAmountBoost = 1;
    public void EnablePowerUp()
    {
        _player.IncreaseProjectilesAmount(_projectilesAmountBoost);
    }

    public void DisablePowerUp()
    {
        _player.ResetProjectileAmount();
    }
}
