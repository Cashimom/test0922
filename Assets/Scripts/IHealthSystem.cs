using UnityEngine;
using System.Collections;

public interface IHealthSystem
{
    public bool Damage(float value);
    public void Recovery(float value);
}