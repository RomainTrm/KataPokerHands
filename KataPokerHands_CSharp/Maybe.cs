using System.Collections.Generic;
using Value;

namespace KataPokerHands_CSharp
{
    public class Maybe<TValue> : ValueType<Maybe<TValue>>
    {
        private readonly bool isNothing;
        private readonly TValue value;

        private Maybe()
        {
            this.isNothing = true;
        }

        private Maybe(TValue value)
        {
            this.value = value;
        }

        public static Maybe<TValue> Nothing() => new Maybe<TValue>();

        public static Maybe<TValue> Just(TValue value) => new Maybe<TValue>(value);

        public static implicit operator Maybe<TValue>(TValue value) => Just(value);

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] { isNothing, value };
        }
    }
}
