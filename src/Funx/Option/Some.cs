using System;

namespace Funx.Option {
    public class Some<T> {
        internal T Value { get; }

        internal Some(T value) {

            if (value == null)
                throw new ArgumentNullException();

            this.Value = value;
        }
    }
}
