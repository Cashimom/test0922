using UnityEngine;
using System.Collections;

public interface IStatusSystem : IEnergySystem, IHealthSystem
{
    float attack { get; set; }
    float defence { get; set; }
    float weight { get; set; }
    float maxHealth { get; set; }
    float maxEnergy { get; set; }


}