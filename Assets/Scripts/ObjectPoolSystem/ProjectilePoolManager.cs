using UnityEngine;

public enum ProjectileType
{
    Player,
    Enemy
}
public class ProjectilePoolManager : ObjectPoolManagerBase<Projectile, ProjectileType>
{
    public static new ProjectilePoolManager Instance
    {
        get
        {
            return Singleton<ProjectilePoolManager>.Instance;
        }
    }
}
