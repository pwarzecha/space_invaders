using UnityEngine;

public class ProjectileSpeedPowerUpBehaviour : MonoBehaviour, IPowerUpBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _speedBoostAmount;

    public void EnablePowerUp()
    {
        _player.IncreaseProjectileSpeed(_speedBoostAmount);
    }
    public void DisablePowerUp()
    {
        _player.ResetProjectileSpeed();
    }

}
