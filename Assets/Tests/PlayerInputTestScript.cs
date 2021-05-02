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
        // A Test behaves as an ordinary method
        [Test]
        public void PlayerInputTestScriptSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator PlayerInputTestScriptWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            var go = new GameObject();
            var pi=go.AddComponent<Players.PlayerInput>();

            pi.moveDirection.Subscribe(vec3 => {
                Assert.That(vec3.z>0.1);
            });

            yield return new MonoBehaviourTest<MyMonoBehaviourTest>();
            //yield return new WaitForFixedUpdate();

            yield return null;
        }

        public class MyMonoBehaviourTest : MonoBehaviour, IMonoBehaviourTest
        {
            private int frameCount;
            public bool IsTestFinished
            {
                get { return frameCount > 1000; }
            }

            void Update()
            {
                frameCount++;
            }
        }
    }
}
