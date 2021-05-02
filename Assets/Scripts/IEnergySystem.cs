using UnityEngine;
using System.Collections;

public interface IEnergySystem
{
    public bool Consume(float value);
    public bool CanConsume(float value);
    public void Charge(float value);
}