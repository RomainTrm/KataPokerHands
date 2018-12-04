using NFluent;
using NUnit.Framework;

namespace KataPokerHands_CSharp
{
    [TestFixture]
    public class PokerHandShould
    {
        private static readonly Card SmallCard = new Card(Values.Two);
        private static readonly Card BigCard = new Card(Values.Ace);

        private static readonly (PokerHand, PokerHand)[] FirstHandWins =
        {
            // Compare on bigest card
            (new PokerHand(new Card(Values.King), SmallCard), new PokerHand(new Card(Values.Queen), SmallCard)),
            (new PokerHand(new Card(Values.Ace), SmallCard), new PokerHand(SmallCard, new Card(Values.Ten))),
            (new PokerHand(SmallCard, new Card(Values.Ace)), new PokerHand(new Card(Values.Ten), SmallCard)),
            (new PokerHand(SmallCard, new Card(Values.Ace)), new PokerHand(SmallCard, new Card(Values.Ten))),

            // Compare on lowest card
            (new PokerHand(new Card(Values.King), BigCard), new PokerHand(new Card(Values.Queen), BigCard)),
            (new PokerHand(BigCard, new Card(Values.King)), new PokerHand(new Card(Values.Queen), BigCard)),
            (new PokerHand(new Card(Values.King), BigCard), new PokerHand(BigCard, new Card(Values.Queen))),
            (new PokerHand(BigCard, new Card(Values.King)), new PokerHand(BigCard, new Card(Values.Queen))),

            // One hand have two cards with same weight
            (new PokerHand(new Card(Values.King), new Card(Values.King)), new PokerHand(SmallCard, new Card(Values.King))),
            (new PokerHand(new Card(Values.King), new Card(Values.King)), new PokerHand(new Card(Values.King), SmallCard)),
        };

        [Test]
        [TestCaseSource(nameof(FirstHandWins))]
        public void ReturnFirstHandWhenTheBestOfTwoHands((PokerHand, PokerHand) @case)
        {
            (PokerHand winningHand, PokerHand losingHand) = @case;
            var expectedResult = Maybe<PokerHand>.Just(winningHand);

            var result1 = PokerHand.GetBestHand(winningHand, losingHand);
            Check.That(result1).IsEqualTo(expectedResult);

            var result2 = PokerHand.GetBestHand(losingHand, winningHand);
            Check.That(result2).IsEqualTo(expectedResult);
        }

        [Test]
        public void ReturnNothingWhenBothHandsHaveSameWeight()
        {
            var result = PokerHand.GetBestHand(new PokerHand(SmallCard, BigCard), new PokerHand(BigCard, SmallCard));
            Check.That(result).IsEqualTo(Maybe<PokerHand>.Nothing());
        }
    }
}