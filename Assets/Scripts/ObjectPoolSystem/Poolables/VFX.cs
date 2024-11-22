using UnityEngine;
public class VFX : MonoBehaviour, IPoolable
{
    [SerializeField] private VFXType _type;
    [SerializeField] private ParticleSystem _particleSystem;

    public void OnCreated()
    {
    }

    public void OnPooled()
    {
        if (_particleSystem != null)
        {
            _particleSystem.Play();
            Invoke(nameof(ReturnToPool), _particleSystem.main.duration + _particleSystem.main.startLifetime.constantMax);
        }
    }

    public void OnReturn()
    {
        if (_particleSystem != null)
        {
            _particleSystem.Stop();
        }
        CancelInvoke(); 
    }

    private void ReturnToPool()
    {
        VFXPoolManager.Instance.Return(_type, this);
    }
}
