namespace AirportLib
{
    public class Belt<T>
    {
        public int Length { get; private set; }
        public T[] Buffer { get; internal set; }

        public Belt(int length)
        {
            Length = length;
            Buffer = new T[length];
            Clear();
        }

        public T this[int i]
        {
            get => Buffer[i];
            internal set => Buffer[i] = value;
        }

        internal void Clear()
        {
            for (int i = 0; i < Length; i++)
            {
                Buffer[i] = default;
            }
        }

        internal void Push(T obj)
        {
            Buffer[0] = obj;
        }

        internal T Pull()
        {
            T obj = Buffer[Length - 1];
            if (obj == null)
            {
                return default;
            } else
            {
                Buffer[Length - 1] = default;
                MoveForward();
                return obj;
            }
        }

        internal void MoveForward()
        {
            if (Buffer[Length - 1] == null)
            {
                for (int i = Length - 1; i > 0; i--)
                {
                    Buffer[i] = Buffer[i - 1];
                }
                Buffer[0] = default;
            }
        }

        public bool IsSpace()
        {
            if (Buffer[0] == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsPullEmpty()
        {
            T type = Buffer[Length - 1];
            if (type == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
