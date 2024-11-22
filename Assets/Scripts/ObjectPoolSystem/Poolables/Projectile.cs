using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable, IDamageable
{
    [SerializeField] private ProjectileSettingsSO _projectileSettingsSO;
    [SerializeField] private ProjectileType _type;
    private int _damage;
    private Vector3 _velocity;
    private Vector2 screenBoundsX;
    private Vector2 screenBoundsY;
    public void Initialize(Vector3 initPosition, Vector3 direction, int damageBoost, float speedBoost)
    {
        transform.position = initPosition;
        _damage = _projectileSettingsSO.baseDamage + damageBoost;
        _velocity = direction.normalized * (_projectileSettingsSO.baseSpeed + speedBoost);
    }

    public void OnCreated()
    {
        screenBoundsX = GameController.Instance.GameSettingsSO.screenBoundsX;
        screenBoundsY = GameController.Instance.GameSettingsSO.screenBoundsY;
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

        if (transform.position.x < screenBoundsX.x || transform.position.x > screenBoundsX.y
            || transform.position.y < screenBoundsY.x || transform.position.y > screenBoundsY.y)
            ProjectilePoolManager.Instance.Return(_type, this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.Hit(_damage);
            ProjectilePoolManager.Instance.Return(_type, this);
            var vfx = VFXPoolManager.Instance.Get(VFXType.OnHit);
            vfx.transform.position = transform.position;
        }
    }
    public void Hit(int damage)
    {
        ProjectilePoolManager.Instance.Return(_type, this);
    }
}
