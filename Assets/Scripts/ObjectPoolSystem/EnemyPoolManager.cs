using UnityEngine;
public enum EnemyType
{
    Basic,
    Armored
}
public class EnemyPoolManager : ObjectPoolManagerBase<Enemy, EnemyType>
{

}
