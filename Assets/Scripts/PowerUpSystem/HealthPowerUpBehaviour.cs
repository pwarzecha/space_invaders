using UnityEngine;


public class HealthPowerUpBehaviour : MonoBehaviour, IPowerUpBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private int _healthAmount = 1;

    public void EnablePowerUp()
    {
        _player.TryHealUp(_healthAmount);
    }
    public void DisablePowerUp()
    {
        
    }

}
