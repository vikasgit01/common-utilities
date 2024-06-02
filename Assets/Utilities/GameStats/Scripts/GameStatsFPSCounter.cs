using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtills
{
    public class GameStatsFpsCounter : MonoBehaviour
    {
        private int lastFrameIndex;
        private float[] frameDeltaTimeArray;

        private void Awake()
        {
            frameDeltaTimeArray = new float[50];
        }

        public string OnUpdate()
        {
            frameDeltaTimeArray[lastFrameIndex] = Time.deltaTime;
            lastFrameIndex = (lastFrameIndex + 1) % frameDeltaTimeArray.Length;

            return Mathf.RoundToInt(CalculateFPS()).ToString();
        }

        private float CalculateFPS()
        {
            float total = 0f;
            foreach (var frame in frameDeltaTimeArray)
            {
                total += frame;
            }

            return frameDeltaTimeArray.Length / total;
        }
    }
}
