using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class Init
    {
        [RuntimeInitializeOnLoadMethod]
        public static void Intialize()
        {
            DOTween.SetTweensCapacity(200,100);
        }
    }
}