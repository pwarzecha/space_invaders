using UnityEngine;
public enum VFXType
{
    Muzzle,
    Explosion,
    OnHit,
    PowerUpPickup
}
public class VFXPoolManager : ObjectPoolManagerBase<VFX, VFXType>
{
    public static new VFXPoolManager Instance
    {
        get
        {
            return Singleton<VFXPoolManager>.Instance;
        }
    }
}
