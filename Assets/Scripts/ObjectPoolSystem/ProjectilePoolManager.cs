using UnityEngine;

public enum ProjectileType
{
    Player,
    Enemy
}
public class ProjectilePoolManager : ObjectPoolManagerBase<Projectile, ProjectileType>
{

}
