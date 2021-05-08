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
        public ReadOnlyReactiveProperty<bool> isFire2=> _isFire2.ToReadOnlyReactiveProperty<bool>();

        private IntReactiveProperty _weaponChange=new IntReactiveProperty();
        public ReadOnlyReactiveProperty<int> weaponChange=> _weaponChange.ToReadOnlyReactiveProperty<int>();



        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            var moveFront = Mathf.Clamp01(Input.GetAxis("Vertical")) * (Input.GetButton("Vertical") ? 1 : 0);
            var moveBack = Mathf.Clamp01(-Input.GetAxis("Vertical")) * (Input.GetButton("Vertical") ? 1 : 0);
            var moveRight = Mathf.Clamp01(Input.GetAxis("Horizontal")) * (Input.GetButton("Horizontal") ? 1 : 0);
            var moveLeft= Mathf.Clamp01(-Input.GetAxis("Horizontal")) * (Input.GetButton("Horizontal") ? 1 : 0);
            _moveDirection.Value = new Vector3(moveRight- moveLeft,0,moveFront-moveBack);

            _rotationDirection.Value = new Vector2(Input.GetAxis("Mouse X") * Time.deltaTime, -Input.GetAxis("Mouse Y") * Time.deltaTime);

            _isRise.Value= Input.GetKey(KeyCode.LeftShift);
            _isUse.Value = Input.GetKey(KeyCode.E);


            if (_isRise.Value && Input.GetButtonDown("Jump"))
            {
                if(_moveDirection.Value!=Vector3.zero)
                    _boostDirection.Value = new Vector3(moveRight - moveLeft, 0, moveFront - moveBack);
                else
                {
                    _boostDirection.Value = new Vector3(0, 1, 0);
                }
            }
            else
            {
                _isJump.Value = Input.GetButtonDown("Jump");

            }

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

            _boostDirection.Value = Vector3.zero;

        }
    }
}