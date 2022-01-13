using System;
using System.Collections.Generic;
using Basement.OEPFramework.UnityEngine.Loop;
using Basement.OEPFramework.UnityEngine._Base;
using UnityEngine;

namespace Basement.OEPFramework.UnityEngine
{
    public sealed class Timer : DroppableItemBase
    {
        public event Action<object> timerParamEvent;
        public event Action timerEvent;
        
        private static readonly List<Timer>[] _add = new List<Timer>[64]; 
        private static readonly List<Timer>[] _remove = new List<Timer>[64]; 
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
                _add[i] = new List<Timer>();
                _remove[i] = new List<Timer>();
            }
        }

        private void Call()
        {
            timerParamEvent?.Invoke(_firedObj);
            timerEvent?.Invoke();
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

        public static Timer Create(float sec, Action<object> method, object obj, IDroppableItem dropper, bool once = false, int engineLoop = -1)
        {
            return new Timer(sec, method, obj, dropper, once, false, engineLoop);
        }

        public static Timer Create(float sec, Action method, IDroppableItem dropper, bool once = false, int engineLoop = -1)
        {
            return new Timer(sec, method, dropper, once, false, engineLoop);
        }

        public static Timer CreateRealtime(float sec, Action<object> method, object obj, IDroppableItem dropper, bool once = false, int engineLoop = -1)
        {
            return new Timer(sec, method, obj, dropper, once, true, engineLoop);
        }

        public static Timer CreateRealtime(float sec, Action method, IDroppableItem dropper, bool once = false, int engineLoop = -1)
        {
            return new Timer(sec, method, dropper, once, true, engineLoop);
        }

        private Timer(float sec, Action<object> method, object obj, IDroppableItem dropper, bool once = false, bool realtime = false, int engineLoop = -1)
        {
            Init(sec, null, method, obj, dropper, once, realtime, engineLoop == -1 ? Loops.TIMER : engineLoop);
        }

        private Timer(float sec, Action method, IDroppableItem dropper, bool once = false, bool realtime = false, int engineLoop = -1)
        {
            Init(sec, method, null, null, dropper, once, realtime, engineLoop == -1 ? Loops.TIMER : engineLoop);
        }

        private void Init(float sec, Action method, Action<object> methodParam, object param, IDroppableItem timerDropper, bool onceCall, bool realtimeTimer, int engineLoop)
        {
            if (method != null)
            {
                timerEvent += method;
            }

            if (methodParam != null)
            {
                timerParamEvent += methodParam;
            }

            _once = onceCall;
            _realtime = realtimeTimer;
            _firedObj = param;
            _engineLoop = engineLoop;

            if (Mathf.Approximately(sec, 0))
            {
                Call();

                if (_once)
                {
                    timerParamEvent = null;
                    timerEvent = null;
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
            
            _add[engineLoop].Add(this);
        }

        private static void InnerAdd(int loop)
        {
            if (_add[loop].Count > 0)
            {
                for (int i = 0; i < _add[loop].Count; i++)
                {
                    var timer = _add[loop][i];
                    if (timer.dropped) continue;
                    
                    _timers[loop].Add(timer);
                }
                
                _add[loop].Clear();
            }
        }

        private static void InnerRemove(int loop)
        {
            if (_remove[loop].Count > 0)
            {
                for (int i = 0; i < _remove[loop].Count; i++)
                {
                    var timer = _remove[loop][i];
                    _timers[loop].Remove(timer);
                }
                
                _remove[loop].Clear();
            }
        }

        public static void Refresh()
        {
            for (int i = 0; i < 64; i++)
            {
                InnerAdd(i);
                InnerRemove(i);
            }
        }
        
        public static void Process(int loop, float deltaTime)
        {
            InnerAdd(loop);
            
            var now = DateTime.UtcNow;
            var dt = deltaTime;
            var dtReal = (float)(now - _lastTime).TotalSeconds;
            _lastTime = now;

            for (var i = 0; i < _timers[loop].Count; i++)
            {
                var timer = _timers[loop][i];
                if (timer.dropped) continue;
                timer.TimerProcess(dt, dtReal);
            }

            InnerRemove(loop);
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
                {
                    Drop();
                }
            }
        }

        private void InternalDrop(IDroppableItem droppableItem)
        {
            Drop();
        }

        public override void Drop()
        {
            if (dropped) return;

            if (_dropper != null)
            {
                _dropper.onDrop -= InternalDrop;
            }

            _dropper = null;

            Pause();

            timerParamEvent = null;
            timerEvent = null;

            _remove[_engineLoop].Add(this);

            base.Drop();
        }
    }
}
