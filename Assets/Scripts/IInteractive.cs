using System;
using UnityEngine;

namespace Game
{
    public interface IInteractive
    {
        public event Action EndEvent;
        bool Begin(bool already);
        void InteractiveUpdate(bool left,bool right, Vector2 pos);

        void Abort();
    }
}