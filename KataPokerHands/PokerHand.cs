using System.Collections.Generic;
using Value;

namespace KataPokerHands
{
    public class PokerHand : ValueType<PokerHand>
    {
        private readonly Card card1;
        private readonly Card card2;

        public PokerHand(Card card1, Card card2)
        {
            this.card1 = card1;
            this.card2 = card2;
        }

        private Card GetHighestCard() => card1 > card2 ? card1 : card2;
        private Card GetLowestCard() => card1 < card2 ? card1 : card2;

        public static Maybe<PokerHand> GetBestHand(PokerHand hand1, PokerHand hand2)
        {
            if (hand1.GetHighestCard() > hand2.GetHighestCard())
                return hand1;
            else if (hand1.GetHighestCard() < hand2.GetHighestCard())
                return hand2;
            else if (hand1.GetLowestCard() > hand2.GetLowestCard())
                return hand1;
            else if (hand1.GetLowestCard() < hand2.GetLowestCard())
                return hand2;
            return Maybe<PokerHand>.Nothing();
        }

        public override string ToString() => $"{nameof(PokerHand)} [{card1}; {card2}]";

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] { card1, card2 };
        }
    }

    public class Card : ValueType<Card>
    {
        private readonly Suits suit;
        private readonly Values value;

        public Card(Suits suit, Values value)
        {
            this.suit = suit;
            this.value = value;
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] { suit, value };
        }

        public override string ToString() => $"{nameof(Card)} [{suit}-{value}]";

        public static bool operator >(Card left, Card right) => left.value > right.value;
        public static bool operator <(Card left, Card right) => left.value < right.value;
    }

    public enum Suits
    {
        Clubs, Diamonds, Hearts, Spades
    }

    public enum Values
    {
        Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace
    }
}
