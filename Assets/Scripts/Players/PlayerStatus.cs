using UnityEngine;
using System.Collections;
using UniRx;

namespace Players
{
    //プレイヤーのステータスを保持するコンポーネント
    public class PlayerStatus : MonoBehaviour,IStatusSystem
    {
        [SerializeField] private float _attack = 0f;
        public float attack { get => _attack; set => _attack = value; }

        [SerializeField] private float _defence = 0f;
        public float defence { get => _defence; set => _defence=value; }

        // 100がデフォルト
        [SerializeField] private float _weight = 100f;
        public float weight { get => _weight; set => _weight=value; }

        [SerializeField] private FloatReactiveProperty _health = new FloatReactiveProperty(100);
        public ReadOnlyReactiveProperty<float> health => _health.ToReadOnlyReactiveProperty<float>();

        [SerializeField] private float _maxHealth = 100f;
        public float maxHealth { get => _maxHealth; set => _maxHealth=value; }

        [SerializeField] private FloatReactiveProperty _energy = new FloatReactiveProperty(100);
        public ReadOnlyReactiveProperty<float> energy => _health.ToReadOnlyReactiveProperty<float>();

        [SerializeField] private float _maxEnergy = 0f;
        public float maxEnergy { get => _maxEnergy; set => _maxEnergy=value; }

        public bool CanConsume(float value)
        {
            return _energy.Value >= value;
        }

        public void Charge(float value)
        {
            _energy.Value = Mathf.Min(maxEnergy, _energy.Value + value);
        }

        public bool Consume(float value)
        {
            if (CanConsume(value))
            {
                _energy.Value -= value;
                return true;
            }
            return false;
        }

        public bool Damage(float value)
        {
            _health.Value -= value;
            return _health.Value >= 0;
        }

        public void Recovery(float value)
        {
            _health.Value = Mathf.Min(maxHealth, _health.Value + value);
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}