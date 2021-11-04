using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSim_H2.Simulation.LuggageSorting
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

        internal void Push(T type)
        {
            Buffer[0] = type;
        }

        internal T Pull()
        {
            T type = Buffer[Length - 1];
            if (type == null)
            {
                return default;
            } else
            {
                Buffer[Length - 1] = default;
                MoveForward();
                return type;
            }
        }

        internal void Clear()
        {
            for (int i = 0; i < Length; i++)
            {
                Buffer[i] = default;
            }
        }

        internal void MoveForward()
        {
            if (Buffer[Length - 1] == null)
            {
                for (int i = Length; i > 0; i++)
                {
                    Buffer[i] = Buffer[i - 1];
                }
                Buffer[0] = default;
            }
        }
    }
}
