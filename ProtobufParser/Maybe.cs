using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser
{
    public class Maybe<T>
    {
        private readonly bool hasValue;
        private readonly T value;

        private Maybe(bool hasValue, T value)
        {
            this.hasValue = hasValue;
            this.value = value;
        }

        public static Maybe<T> Some(T value)
        {
            return new Maybe<T>(true, value);
        }

        public static Maybe<T> None()
        {
            return new Maybe<T>(false, default(T));
        }

        public bool HasValue
        {
            get { return hasValue; }
        }

        public T Value
        {
            get { return value; }
        }

        public override bool Equals(object obj)
        {
            var other = obj as Maybe<T>;
            return other != null && ((!other.hasValue && !hasValue) || (other.hasValue && hasValue && other.value.Equals(value)));
        }

        public override int GetHashCode()
        {
            if (hasValue)
            {
                return value.GetHashCode();
            }
            else
            {
                return 0;
            }
        }
    }
}
