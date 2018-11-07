using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimCapi
{
    public class SimCapiTimer
    {

        public delegate void FinishedDelegate();

        float _time;
        float _duration;
        bool _enabled;

        FinishedDelegate _finishedDelegate;

        public SimCapiTimer(float duration, FinishedDelegate finishedDelegate)
        {
            _duration = duration;
            _enabled = false;
            _finishedDelegate = finishedDelegate;
        }

        public void update(float deltaTime)
        {
            if (_enabled == false)
                return;

            _time -= deltaTime;

            if (_time <= 0 && _enabled == true)
            {
                _enabled = false;

                if (_finishedDelegate != null)
                    _finishedDelegate();
            }
        }

        public bool enabled()
        {
            return _enabled;
        }

        public void start()
        {
            if (_duration <= 0)
            {
                if (_finishedDelegate != null)
                    _finishedDelegate();

                return;
            }

            _time = _duration;
            _enabled = true;
        }



    }
}
