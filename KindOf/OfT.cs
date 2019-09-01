using System;

namespace KindOf
{
    public abstract class Of<T>
    {
        protected readonly T Value;

        public Of(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
        }

        public override bool Equals(object obj) =>
            obj != null &&
            obj.GetType() == GetType() &&
            Equals(Value, ((Of<T>)obj).Value);

        public override int GetHashCode() =>
            GetType().GetHashCode() * 31 + Value.GetHashCode();

        public static implicit operator T(Of<T> of) => of.Value;
    }
}
