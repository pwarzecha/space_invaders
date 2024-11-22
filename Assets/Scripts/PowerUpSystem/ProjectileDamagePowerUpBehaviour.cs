using UnityEngine;


public class ProjectileDamagePowerUpBehaviour : MonoBehaviour, IPowerUpBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private int _damageBoostAmount;

    public void EnablePowerUp()
    {
        _player.IncreaseProjectileDamage(_damageBoostAmount);
    }
    public void DisablePowerUp()
    {
        _player.ResetProjectileDamage();
    }

}
