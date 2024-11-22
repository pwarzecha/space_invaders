using UnityEngine;
public enum PowerUpType
{
    BulletSpeed,
    BulletDamage,
    BulletAmount,
    Heal,
}
public class PowerUpPoolManager : ObjectPoolManagerBase<PowerUp, PowerUpType>
{
}