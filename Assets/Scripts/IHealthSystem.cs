using UnityEngine;
using System.Collections;
using UniRx;

public interface IHealthSystem
{
    ReadOnlyReactiveProperty<float> health { get; }
    bool Damage(float value);
    void Recovery(float value);
}