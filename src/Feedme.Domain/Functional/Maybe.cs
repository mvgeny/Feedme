using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Feedme.Domain.Functional
{
    public class Maybe<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> values;

        public T Value
        {
            get
            {
                if (HasNoValue)
                    throw new InvalidOperationException();
                return values.Single();
            }
        }
        public bool HasValue => values.Any();
        public bool HasNoValue => !HasValue;
        public Maybe()
        {
            this.values = new T[0];
        }

        public Maybe(T value)
        {
            this.values = new[] { value };
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}