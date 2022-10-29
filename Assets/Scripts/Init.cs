using DG.Tweening;
using UnityEditor;

namespace Game
{
    public class Init
    {
        [InitializeOnLoadMethod]
        public static void Intialize()
        {
            DOTween.SetTweensCapacity(200,100);
        }
    }
}