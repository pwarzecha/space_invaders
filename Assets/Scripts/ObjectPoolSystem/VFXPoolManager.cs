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

}
