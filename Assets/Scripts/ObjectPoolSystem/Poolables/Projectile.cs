using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable
{
    [SerializeField] private ProjectileType _type;
    [SerializeField] private ProjectileSettingsSO _projectileSettingsSO;
    private int _damage;
    private Vector3 _velocity;
    public void Initialize(Vector3 initPosition, Vector3 direction, int damageBoost, float speedBoost)
    {
        transform.position = initPosition;
        _damage = _projectileSettingsSO.baseDamage + damageBoost;
        _velocity = direction.normalized * (_projectileSettingsSO.baseSpeed + speedBoost);
    }
    public void OnCreated()
    {

    }

    public void OnPooled()
    {

    }

    public void OnReturn()
    {
        _damage = 0;
        _velocity = Vector3.zero;
    }

    private void Update()
    {
        transform.position += _velocity * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.Hit(_damage);
            ProjectilePoolManager.Instance.Return(_type, this);
        }
        else if (other.TryGetComponent(out Player player))
        {
            player.Hit(_damage);
            ProjectilePoolManager.Instance.Return(_type, this);
        }
    }
}
