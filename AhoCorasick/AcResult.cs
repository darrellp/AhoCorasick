using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhoCorasick
{
    public struct AcResult<T>
    {
        public int Position { get; private set; }
        public T Value { get; private set; }
        public AcResult(int position, T value)
        {
            Position = position;
            Value = value;
        }
    }
}
