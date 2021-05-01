using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Players
{

    public class PlayerInput : MonoBehaviour
    {

        private Vector3ReactiveProperty _moveDirection;
        public ReadOnlyReactiveProperty<Vector3> moveDirection
        {
            get { return _moveDirection.ToReadOnlyReactiveProperty<Vector3>(); }
        }

        private Vector3ReactiveProperty _boostDirection;
        public ReadOnlyReactiveProperty<Vector3> boostDirection
        {
            get { return _boostDirection.ToReadOnlyReactiveProperty<Vector3>(); }
        }

        private Vector2ReactiveProperty _rotationDirection;
        public ReadOnlyReactiveProperty<Vector2> rotationDirection
        {
            get { return _rotationDirection.ToReadOnlyReactiveProperty<Vector2>(); }
        }

        private BoolReactiveProperty _isRise;
        public ReadOnlyReactiveProperty<bool> isRise
        {
            get { return _isRise.ToReadOnlyReactiveProperty<bool>(); }
        }

        private BoolReactiveProperty _isUse;
        public ReadOnlyReactiveProperty<bool> isUse
        {
            get { return _isUse.ToReadOnlyReactiveProperty<bool>(); }
        }

        private BoolReactiveProperty _isJump;
        public ReadOnlyReactiveProperty<bool> isJump
        {
            get { return _isJump.ToReadOnlyReactiveProperty<bool>(); }
        }

        private BoolReactiveProperty _isFire1;
        public ReadOnlyReactiveProperty<bool> isFire1
        {
            get { return _isFire1.ToReadOnlyReactiveProperty<bool>(); }
        }

        private BoolReactiveProperty _isFire2;
        public ReadOnlyReactiveProperty<bool> isFire2
        {
            get { return isFire2.ToReadOnlyReactiveProperty<bool>(); }
        }

        private IntReactiveProperty _weaponChange;
        public ReadOnlyReactiveProperty<int> weaponChange
        {
            get { return _weaponChange.ToReadOnlyReactiveProperty<int>(); }
        }



        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            _moveDirection.Value = new Vector3(
                Mathf.Clamp01(Input.GetAxis("Vertical")) * (Input.GetButton("Vertical") ? 1 : 0)
                - Mathf.Clamp01(-Input.GetAxis("Vertical")) * (Input.GetButton("Vertical") ? 1 : 0),
                0,
                Mathf.Clamp01(Input.GetAxis("Horizontal")) * (Input.GetButton("Horizontal") ? 1 : 0)
                - Mathf.Clamp01(-Input.GetAxis("Horizontal")) * (Input.GetButton("Horizontal") ? 1 : 0));

            _isRise.Value= Input.GetKey(KeyCode.LeftShift);
            _isJump.Value = Input.GetButtonDown("Jump");
            _isUse.Value = Input.GetKey(KeyCode.E);

            if (Input.GetButtonDown("Fire1"))
            {
                _isFire1.Value = true;
            }
            if (Input.GetButtonUp("Fire1"))
            {
                _isFire1.Value = false;
            }

            if (Input.GetButtonDown("Fire2"))
            {
                _isFire2.Value = true;
            }
            if (Input.GetButtonUp("Fire2"))
            {
                _isFire2.Value = false;
            }

            _weaponChange.Value = (int)Mathf.Sign(Input.GetAxis("Mouse ScrollWheel"));

        }
    }
}