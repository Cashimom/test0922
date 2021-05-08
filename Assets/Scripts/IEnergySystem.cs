using UnityEngine;
using System.Collections;
using UniRx;

public interface IEnergySystem
{
    ReadOnlyReactiveProperty<float> energy { get; }
    bool Consume(float value);
    bool CanConsume(float value);
    void Charge(float value);
}