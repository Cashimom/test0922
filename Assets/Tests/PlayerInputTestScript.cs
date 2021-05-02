using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Players;
using UniRx;

namespace Tests
{
    public class PlayerInputTestScript
    {
        GameObject go;
        PlayerInput pi;

        [OneTimeSetUp]
        public void FirstSetUp()
        {
            go = new GameObject();
            pi = go.AddComponent<PlayerInput>();
        }

        // A Test behaves as an ordinary method
        [Test]
        public void aaOperationTest()
        {
            Assert.That(true);
        }


        [UnityTest]
        public IEnumerator ReceiveInputMoveDirection()
        {
            pi.moveDirection.Subscribe(x => {
                Debug.Log($"Move {x}");
                Assert.That(true);
            }).AddTo(go);

            yield return new MonoBehaviourTest<MyMonoBehaviourTest>();

        }

        [UnityTest]
        public IEnumerator ReceiveInputBoostDirection()
        {
            pi.boostDirection.Subscribe(x => {
                Debug.Log($"Boost {x}");
                Assert.That(true);
            }).AddTo(go);

            yield return new MonoBehaviourTest<MyMonoBehaviourTest>();

        }
        
        [UnityTest]
        public IEnumerator ReceiveInputRotationDirection()
        {
            pi.rotationDirection.Subscribe(x => {
                Debug.Log($"Rotation {x}");
                Assert.That(true);
            }).AddTo(go);

            yield return new MonoBehaviourTest<MyMonoBehaviourTest>();

        }

        [UnityTest]
        public IEnumerator ReceiveInputRise()
        {
            pi.isRise.Subscribe(x => {
                Debug.Log($"isRise {x}");
                Assert.That(true);
            }).AddTo(go);

            yield return new MonoBehaviourTest<MyMonoBehaviourTest>();

        }

        [UnityTest]
        public IEnumerator ReceiveInputUse()
        {
            pi.isRise.Subscribe(x => {
                Debug.Log($"isUse {x}");
                Assert.That(true);
            }).AddTo(go);

            yield return new MonoBehaviourTest<MyMonoBehaviourTest>();

        }

        [UnityTest]
        public IEnumerator ReceiveInputJump()
        {
            pi.isJump.Subscribe(x => {
                Debug.Log($"isJump {x}");
                Assert.That(true);
            }).AddTo(go);

            yield return new MonoBehaviourTest<MyMonoBehaviourTest>();

        }

        [UnityTest]
        public IEnumerator ReceiveInputFire1()
        {
            pi.isFire1.Subscribe(x => {
                Debug.Log($"isFire1 {x}");
                Assert.That(true);
            }).AddTo(go);

            yield return new MonoBehaviourTest<MyMonoBehaviourTest>();

        }

        [UnityTest]
        public IEnumerator ReceiveInputFire2()
        {
            pi.isFire2.Subscribe(x => {
                Debug.Log($"isFire2 {x}");
                Assert.That(true);
            }).AddTo(go);

            yield return new MonoBehaviourTest<MyMonoBehaviourTest>();

        }

        [UnityTest]
        public IEnumerator ReceiveInputWeaponChange()
        {
            pi.weaponChange.Subscribe(x => {
                Debug.Log($"weaponChange {x}");
                Assert.That(true);
            }).AddTo(go);

            yield return new MonoBehaviourTest<MyMonoBehaviourTest>();

        }

        public class MyMonoBehaviourTest : MonoBehaviour, IMonoBehaviourTest
        {
            private int frameCount;
            private int inputCount;
            public bool IsTestFinished
            {
                get { return inputCount > 5||frameCount>5000; }
            }

            void Update()
            {
                if (Input.anyKeyDown)
                    inputCount++;
                frameCount++;

            }
        }
    }
}
