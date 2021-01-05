using System;
using CoreFoundation;

namespace NDB.Covid19.iOS.Utils
{
    public class StressUtils
    {
        public class GenericSingleAction<T1, T2>
        {
            private bool _hasStarted;
            private readonly Action<T1, T2> _setOnAction;
            private int _delayMilliseconds;

            public GenericSingleAction(Action<T1, T2> setOnAction, int delayMilliseconds = 1000)
            {
                _setOnAction = setOnAction;
                _delayMilliseconds = delayMilliseconds;
            }

            public void Run(T1 v, T2 e)
            {
                if (!_hasStarted)
                {
                    _hasStarted = true;
                    _setOnAction?.Invoke(v, e);
                }

                Reset();
            }

            private void Reset()
            {
                DispatchQueue.MainQueue.DispatchAfter(new DispatchTime(DispatchTime.Now, _delayMilliseconds * 10000000), () => { _hasStarted = false; });
            }
        }

        public class SingleAction<T> : GenericSingleAction<object, T>
        {
            public SingleAction(Action<object, T> setOnAction, int delayMilliseconds = 1000) : base(setOnAction, delayMilliseconds)
            {
            }
        }

        public class SingleAction
        {
            private bool _hasStarted;
            private readonly Action _setOnAction;
            private int _delayMilliseconds;

            public SingleAction(Action setOnAction, int delayMilliseconds = 1000)
            {
                _setOnAction = setOnAction;
                _delayMilliseconds = delayMilliseconds;
            }

            public void Run()
            {
                if (!_hasStarted)
                {
                    _hasStarted = true;
                    _setOnAction?.Invoke();
                }

                Reset();
            }

            private void Reset()
            {
                DispatchQueue.MainQueue.DispatchAfter(new DispatchTime(DispatchTime.Now, _delayMilliseconds * 10000000), () => { _hasStarted = false; });
            }
        }

        public class SingleClick : SingleAction<EventArgs>
        {
            public SingleClick(Action<object, EventArgs> setOnAction, int delayMilliseconds = 1000) : base(setOnAction, delayMilliseconds)
            {
            }
        }

        public class SinglePress : SingleAction<bool>
        {
            public SinglePress(Action<object, bool> setOnAction, int delayMilliseconds = 1000) : base(setOnAction, delayMilliseconds)
            {
            }
        }
    }
}
