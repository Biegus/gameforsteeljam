using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    public class Helper
    {
        public static Quaternion ZRot(float angle)
        {
            return Quaternion.Euler(0,0,angle);
        }

        public static IEnumerator Untill(Func<bool> func)
        {
            while (!func())
                yield return null;
        }
    }
}