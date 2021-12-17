namespace AirportLib
{
    /// <summary>
    /// A Belt with a type
    /// </summary>
    /// <typeparam name="T"></typeparam>
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

        /// <summary>
        /// Clears the Buffer
        /// </summary>
        internal void Clear()
        {
            for (int i = 0; i < Length; i++)
            {
                Buffer[i] = default;
            }
        }

        /// <summary>
        /// Set "start of belt" to obj
        /// </summary>
        /// <param name="obj"></param>
        internal void Push(T obj)
        {
            Buffer[0] = obj;
        }

        /// <summary>
        /// Remove last obj
        /// </summary>
        /// <returns>last obj</returns>
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

        /// <summary>
        /// Moves all items 1 forward, like a belt
        /// </summary>
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

        /// <summary>
        /// Check if index 0 is empty
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Check if last obj is empty
        /// </summary>
        /// <returns></returns>
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
