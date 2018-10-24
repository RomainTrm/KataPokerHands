using NFluent;
using NUnit.Framework;

namespace KataPokerHands
{
    [TestFixture]
    public class PokerHandShould
    {
        private static readonly Card SmallCard = new Card(Suits.Hearts, Values.Two);
        private static readonly Card BigCard = new Card(Suits.Hearts, Values.Ace);

        private static readonly (PokerHand, PokerHand)[] FirstHandWins =
        {
            (new PokerHand(new Card(Suits.Clubs, Values.King), SmallCard), new PokerHand(new Card(Suits.Spades, Values.Queen), SmallCard)),
            (new PokerHand(new Card(Suits.Clubs, Values.Ace), SmallCard), new PokerHand(SmallCard, new Card(Suits.Spades, Values.Ten))),
            (new PokerHand(SmallCard, new Card(Suits.Clubs, Values.Ace)), new PokerHand(new Card(Suits.Spades, Values.Ten), SmallCard)),
            (new PokerHand(SmallCard, new Card(Suits.Clubs, Values.Ace)), new PokerHand(SmallCard, new Card(Suits.Spades, Values.Ten))),
            (new PokerHand(new Card(Suits.Clubs, Values.King), BigCard), new PokerHand(new Card(Suits.Spades, Values.Queen), BigCard)),
            (new PokerHand(BigCard, new Card(Suits.Clubs, Values.King)), new PokerHand(new Card(Suits.Spades, Values.Queen), BigCard)),
            (new PokerHand(new Card(Suits.Clubs, Values.King), BigCard), new PokerHand(BigCard, new Card(Suits.Spades, Values.Queen))),
            (new PokerHand(BigCard, new Card(Suits.Clubs, Values.King)), new PokerHand(BigCard, new Card(Suits.Spades, Values.Queen))),
        };

        [Test]
        [TestCaseSource(nameof(FirstHandWins))]
        public void ReturnFirstHandWhenTheBestOfTwoHands((PokerHand, PokerHand) @case)
        {
            (PokerHand winningHand, PokerHand losingHand) = @case;

            var result1 = PokerHand.GetBestHand(winningHand, losingHand);
            Check.That(result1).IsEqualTo(winningHand);

            var result2 = PokerHand.GetBestHand(losingHand, winningHand);
            Check.That(result2).IsEqualTo(winningHand);
        }
    }
}
