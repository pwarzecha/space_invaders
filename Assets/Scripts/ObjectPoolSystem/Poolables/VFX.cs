using System;
using System.Threading.Tasks;
using UnityEngine;
public class VFX : MonoBehaviour, IPoolable
{
    [SerializeField] private VFXType _type;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private bool returnAfterPlay;
    public void OnCreated()
    {
        gameObject.SetActive(false);
    }

    public void OnPooled()
    {
        if (_particleSystem != null)
        {
            _particleSystem.Play();
            if (returnAfterPlay)
                WaitForParticleEnd();
        }
    }

    public void OnReturn()
    {
        if (_particleSystem != null)
        {
            _particleSystem.Stop();
            _particleSystem.Clear();
        }
    }
    private async Task WaitForParticleEnd()
    {
        var mainModule = _particleSystem.main;
        if (mainModule.loop)
        {
            Debug.LogWarning("Cannot await emission end on a looping ParticleSystem.");
            return;
        }
        while (_particleSystem.IsAlive(true))
        {
            await Task.Yield();
        }
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        VFXPoolManager.Instance.Return(_type, this);
    }
}
