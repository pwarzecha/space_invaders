using UnityEngine;
public enum PowerUpType
{
    FireRate,
    ProjectileDamage,
    ProjectileAmount,
    Heal,
}
public class PowerUpPoolManager : ObjectPoolManagerBase<PowerUp, PowerUpType>
{
    public static new PowerUpPoolManager Instance
    {
        get
        {
            return Singleton<PowerUpPoolManager>.Instance;
        }
    }
}