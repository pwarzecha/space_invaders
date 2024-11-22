using System;
using UnityEngine;
public interface IPoolable
{
    void OnCreated();
    void OnPooled();
    void OnReturn();

}