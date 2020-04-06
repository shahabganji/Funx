using System;
using System.Collections.Generic;

namespace Funx.Validation
{
    public readonly struct Valid<T>
    {
        internal T Data { get;}
        
        internal  Valid( T data) => this.Data = data ?? throw new ArgumentNullException(nameof(data));
    }

}
