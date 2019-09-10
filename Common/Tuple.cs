namespace Basement.Common
{
    public class Tuple<T1, T2>
    {
        public T1 value1 { get; }
        public T2 value2 { get; }

        public Tuple(T1 value1, T2 value2)
        {
            this.value1 = value1;
            this.value2 = value2;
        }
    }

    public class Tuple<T1, T2, T3> : Tuple<T1, T2>
    {
        public T3 value3 { get; }

        public Tuple(T1 value1, T2 value2, T3 value3) : base(value1, value2)
        {
            this.value3 = value3;
        }
    }

    public class Tuple<T1, T2, T3, T4> : Tuple<T1, T2, T3>
    {
        public T4 value4 { get; }

        public Tuple(T1 value1, T2 value2, T3 value3, T4 value4) : base(value1, value2, value3)
        {
            this.value4 = value4;
        }
    }

    public class Tuple<T1, T2, T3, T4, T5> : Tuple<T1, T2, T3, T4>
    {
        public T5 value5 { get; }

        public Tuple(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5) : base(value1, value2, value3, value4)
        {
            this.value5 = value5;
        }
    }
}
