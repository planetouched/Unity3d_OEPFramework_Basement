using System;
using System.Collections.Generic;
using Basement.OEPFramework.UnityEngine.Loop;
using Basement.OEPFramework.UnityEngine._Base;
using Basement.OEPFramework.UnityEngine.Loop.TimeData;
using UnityEngine;

namespace Basement.OEPFramework.UnityEngine
{
    public sealed class Timer : DroppableItemBase
    {
        public delegate void OnTimeUp(object o);
        public delegate void OnTimeUpVoid();
        public OnTimeUp onTimeUpEvent;
        public OnTimeUpVoid onTimeUpVoidEvent;
        
        private static readonly List<Timer>[] _timers = new List<Timer>[64];
        private static DateTime _lastTime;

        private float _timeStep;
        private float _timeElapsed;
        private object _firedObj;
        private bool _work;
        private bool _once;
        private bool _realtime;
        private int _engineLoop;
        private IDroppableItem _dropper;

        static Timer()
        {
            _lastTime = DateTime.UtcNow;

            for (int i = 0; i < 64; i++)
            {
                _timers[i] = new List<Timer>();
            }
        }

        private void Call()
        {
            if (onTimeUpEvent != null)
                onTimeUpEvent(_firedObj);

            if (onTimeUpVoidEvent != null)
                onTimeUpVoidEvent();
        }

        public void Pause()
        {
            _work = false;
        }

        public void Reset()
        {
            _timeElapsed = 0;
        }

        public void Resume()
        {
            _work = true;
        }

        public static Timer Create(float sec, OnTimeUp func, object obj, IDroppableItem dropper, bool once = false, int? engineLoop = null)
        {
            return new Timer(sec, func, obj, dropper, once, false, engineLoop);
        }

        public static Timer Create(float sec, OnTimeUpVoid func, IDroppableItem dropper, bool once = false, int? engineLoop = null)
        {
            return new Timer(sec, func, dropper, once, false, engineLoop);
        }

        public static Timer CreateRealtime(float sec, OnTimeUp func, object obj, IDroppableItem dropper, bool once = false, int? engineLoop = null)
        {
            return new Timer(sec, func, obj, dropper, once, true, engineLoop);
        }

        public static Timer CreateRealtime(float sec, OnTimeUpVoid func, IDroppableItem dropper, bool once = false, int? engineLoop = null)
        {
            return new Timer(sec, func, dropper, once, true, engineLoop);
        }

        private Timer(float sec, OnTimeUp func, object obj, IDroppableItem dropper, bool once = false, bool realtime = false, int? engineLoop = null)
        {
            Init(sec, null, func, obj, dropper, once, realtime, engineLoop ?? Loops.TIMER);
        }

        private Timer(float sec, OnTimeUpVoid func, IDroppableItem dropper, bool once = false, bool realtime = false, int? engineLoop = null)
        {
            Init(sec, func, null, null, dropper, once, realtime, engineLoop ?? Loops.TIMER);
        }

        private void Init(float sec, OnTimeUpVoid funcVoid, OnTimeUp func, object obj, IDroppableItem timerDropper, bool onceCall, bool realtimeTimer, int engineLoop)
        {
            if (funcVoid != null)
                onTimeUpVoidEvent = funcVoid;

            if (func != null)
                onTimeUpEvent = func;

            _once = onceCall;
            _realtime = realtimeTimer;
            _firedObj = obj;
            _engineLoop = engineLoop;

            if (Mathf.Approximately(sec, 0))
            {
                Call();

                if (_once)
                {
                    onTimeUpEvent = null;
                    onTimeUpVoidEvent = null;
                    return;
                }
            }

            _timeStep = sec;
            _work = true;

            if (timerDropper != null)
            {
                _dropper = timerDropper;
                _dropper.onDrop += InternalDrop;
            }

            Sync.Add(() => _timers[_engineLoop].Add(this), _engineLoop);
        }

        public static void Process(int loop, ITimeData timeData)
        {
            Sync.Process(loop);
            var now = DateTime.UtcNow;
            var dt = timeData.deltaTime;
            var dtReal = (float)(now - _lastTime).TotalSeconds;
            _lastTime = now;
            
            foreach (var task in _timers[loop])
                task.TimerProcess(dt, dtReal);
        }

        private void TimerProcess(float dt, float dtReal)
        {
            if (!_work) return;
            _timeElapsed += !_realtime ? dt : dtReal;
            if (_timeElapsed >= _timeStep)
            {
                _timeElapsed -= _timeStep;

                Call();

                if (_once)
                    Drop();
            }
        }

        private void InternalDrop(IDroppableItem obj)
        {
            Drop();
        }

        public override void Drop()
        {
            if (dropped) return;

            if (_dropper != null)
                _dropper.onDrop -= InternalDrop;
            _dropper = null;

            Pause();

            onTimeUpEvent = null;
            onTimeUpVoidEvent = null;

            Sync.Add(() => _timers[_engineLoop].Remove(this), _engineLoop);

            base.Drop();
        }
    }
}
