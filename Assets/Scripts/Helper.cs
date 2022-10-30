using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Game
{
    public static class Helper
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

        public static void DrawGraphOnScene(this GameObject g,float startX, AnimationCurve curve,  float maxY,float minY,float maxX)
        {


            Vector2 Get(float x)
            {
                return new Vector2(x + startX,
                    curve.Evaluate(x / 32) * (maxY - minY) + minY);
            }
            for (int i = 0; i <= 100; i++)
            {
                float x =   (maxX - startX) * i / ((float) 100);
                float x2 =  (maxX - startX) * (i+1) / ((float) 100);
                Gizmos.DrawLine(Get(x), Get(x2));
            } 
        }
    }
}