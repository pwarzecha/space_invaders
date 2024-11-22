using UnityEngine;

public class FireRatePowerUpBehaviour : MonoBehaviour, IPowerUpBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _fireRateBoostAmount;

    public void EnablePowerUp()
    {
        _player.IncreaseFireRate(_fireRateBoostAmount);
    }
    public void DisablePowerUp()
    {
        _player.ResetFireRate();
    }

}
