using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Players
{

    public class PlayerInput : MonoBehaviour
    {

        private Vector3ReactiveProperty _moveDirection= new Vector3ReactiveProperty();
        public ReadOnlyReactiveProperty<Vector3> moveDirection => _moveDirection.ToReadOnlyReactiveProperty<Vector3>();

        private Vector3ReactiveProperty _boostDirection= new Vector3ReactiveProperty();
        public ReadOnlyReactiveProperty<Vector3> boostDirection=> _boostDirection.ToReadOnlyReactiveProperty<Vector3>();

        private Vector2ReactiveProperty _rotationDirection=new Vector2ReactiveProperty();
        public ReadOnlyReactiveProperty<Vector2> rotationDirection=> _rotationDirection.ToReadOnlyReactiveProperty<Vector2>();

        private BoolReactiveProperty _isRise=new BoolReactiveProperty();
        public ReadOnlyReactiveProperty<bool> isRise=> _isRise.ToReadOnlyReactiveProperty<bool>();

        private BoolReactiveProperty _isUse=new BoolReactiveProperty();
        public ReadOnlyReactiveProperty<bool> isUse=> _isUse.ToReadOnlyReactiveProperty<bool>();

        private BoolReactiveProperty _isJump=new BoolReactiveProperty();
        public ReadOnlyReactiveProperty<bool> isJump=> _isJump.ToReadOnlyReactiveProperty<bool>();

        private BoolReactiveProperty _isFire1=new BoolReactiveProperty();
        public ReadOnlyReactiveProperty<bool> isFire1=> _isFire1.ToReadOnlyReactiveProperty<bool>();

        private BoolReactiveProperty _isFire2=new BoolReactiveProperty();
        public ReadOnlyReactiveProperty<bool> isFire2=> isFire2.ToReadOnlyReactiveProperty<bool>();

        private IntReactiveProperty _weaponChange=new IntReactiveProperty();
        public ReadOnlyReactiveProperty<int> weaponChange=> _weaponChange.ToReadOnlyReactiveProperty<int>();



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