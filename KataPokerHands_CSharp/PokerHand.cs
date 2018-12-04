using System.Collections.Generic;
using Value;

namespace KataPokerHands_CSharp
{
    public class PokerHand : ValueType<PokerHand>
    {
        private readonly Card _card1;
        private readonly Card _card2;

        public PokerHand(Card card1, Card card2)
        {
            _card1 = card1;
            _card2 = card2;
        }

        public Card GetHighestCard() => _card1 > _card2 ? _card1 : _card2;
        public Card GetLowestCard() => _card1 < _card2 ? _card1 : _card2;

        public static Maybe<PokerHand> GetBestHand(PokerHand hand1, PokerHand hand2)
        {
            if (hand1.GetHighestCard() > hand2.GetHighestCard())
                return hand1;
            if (hand1.GetHighestCard() < hand2.GetHighestCard())
                return hand2;
            if (hand1.GetLowestCard() > hand2.GetLowestCard())
                return hand1;
            if (hand1.GetLowestCard() < hand2.GetLowestCard())
                return hand2;
            return Maybe<PokerHand>.Nothing();
        }

        public override string ToString() => $"{nameof(PokerHand)} [{_card1}; {_card2}]";

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] { _card1, _card2 };
        }
    }

    public class Card : ValueType<Card>
    {
        private readonly Values _value;

        public Card(Values value)
        {
            _value = value;
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] { _value };
        }

        public override string ToString() => $"{nameof(Card)} [{_value}]";

        public static bool operator >(Card left, Card right) => left._value > right._value;
        public static bool operator <(Card left, Card right) => left._value < right._value;
    }
    
    public enum Values
    {
        Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace
    }
}
