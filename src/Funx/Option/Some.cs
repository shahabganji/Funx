using System;

namespace Funx.Option {
    public struct Some<T> {
        internal T Value { get; }

        internal Some(T value) {

            if (value == null)
                throw new ArgumentNullException();

            this.Value = value;
        }
    }
}
