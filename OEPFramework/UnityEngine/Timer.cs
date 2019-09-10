using System;
using System.Collections.Generic;
using Basement.OEPFramework.UnityEngine.Loop;
using Basement.OEPFramework.UnityEngine._Base;
using UnityEngine;

namespace Basement.OEPFramework.UnityEngine
{
    public sealed class Timer : DroppableItemBase
    {
        public delegate void OnTimeUp(object o);
        public delegate void OnTimeUpVoid();
        public OnTimeUp onTimeUpEvent;
        public OnTimeUpVoid onTimeUpVoidEvent;
        
        private static readonly List<Timer> _timers = new List<Timer>();
        private static DateTime _lastTime;

        private float _timeStep;
        private float _timeElapsed;
        private object _firedObj;
        private bool _work;
        private bool _once;
        private bool _realtime;
        private IDroppableItem _dropper;

        static Timer()
        {
            _lastTime = DateTime.UtcNow;
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

        public static Timer Create(float sec, OnTimeUp func, object obj, IDroppableItem dropper, bool once = false)
        {
            return new Timer(sec, func, obj, dropper, once);
        }

        public static Timer Create(float sec, OnTimeUpVoid func, IDroppableItem dropper, bool once = false)
        {
            return new Timer(sec, func, dropper, once);
        }

        public static Timer CreateRealtime(float sec, OnTimeUp func, object obj, IDroppableItem dropper, bool once = false)
        {
            return new Timer(sec, func, obj, dropper, once, true);
        }

        public static Timer CreateRealtime(float sec, OnTimeUpVoid func, IDroppableItem dropper, bool once = false)
        {
            return new Timer(sec, func, dropper, once, true);
        }

        private Timer(float sec, OnTimeUp func, object obj, IDroppableItem dropper, bool once = false, bool realtime = false)
        {
            Init(sec, null, func, obj, dropper, once, realtime);
        }

        private Timer(float sec, OnTimeUpVoid func, IDroppableItem dropper, bool once = false, bool realtime = false)
        {
            Init(sec, func, null, null, dropper, once, realtime);
        }

        private void Init(float sec, OnTimeUpVoid funcVoid, OnTimeUp func, object obj, IDroppableItem timerDropper, bool onceCall, bool realtimeTimer)
        {
            if (funcVoid != null)
                onTimeUpVoidEvent = funcVoid;

            if (func != null)
                onTimeUpEvent = func;

            _once = onceCall;
            _realtime = realtimeTimer;
            _firedObj = obj;

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

            Sync.Add(() => _timers.Add(this), Loops.TIMER);
        }

        public static void Process()
        {
            Sync.Process(Loops.TIMER);
            var now = DateTime.UtcNow;
            var dt = Time.deltaTime;
            var dtReal = (float)(now - _lastTime).TotalSeconds;
            _lastTime = now;

            foreach (var task in _timers)
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

            Sync.Add(() => _timers.Remove(this), Loops.TIMER);

            base.Drop();
        }
    }
}
