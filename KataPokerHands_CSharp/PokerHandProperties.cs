using System;
using System.Linq;
using FsCheck;
using FsCheck.Xunit;

namespace KataPokerHands_CSharp
{
    public class PokerHandProperties
    {
        private static readonly Card Ace = new Card(Values.Ace);
        private static readonly Card Two = new Card(Values.Two);

        [Property]
        public bool AceIsTheBestCard(Values value)
        {
            var card = new Card(value);
            var handLeft = new PokerHand(Ace, card);
            var handRight = new PokerHand(card, Ace);

            return handLeft.GetHighestCard() == Ace 
                && handRight.GetHighestCard() == Ace
                && handLeft.GetLowestCard() == card
                && handRight.GetLowestCard() == card;
        }

        [Property]
        public Property TwoIsTheWorstCard(Card card)
        {
            var handLeft = new PokerHand(Two, card);
            var handRight = new PokerHand(card, Two);

            return (handLeft.GetHighestCard() == card).Label($"The best card is {card} on hand {handLeft}") 
               .And(handRight.GetHighestCard() == card).Label($"The best card is {card} on hand {handRight}")
               .And(handLeft.GetLowestCard() == Two).Label($"The lowest card is {Two} on hand {handLeft}")
               .And(handRight.GetLowestCard() == Two).Label($"The lowest card is {Two} on hand {handRight}");
        }

        [Property]
        public void EachCardHasItsSpecificValue() 
            => Prop.ForAll((Card card1, Card card2) =>
            {
                var cardsByValuesDescending = new[]
                    {
                        Values.Ace, Values.King, Values.Queen, Values.Jack, Values.Ten, Values.Nine, Values.Eight, Values.Seven, Values.Six, Values.Five, Values.Four, Values.Three, Values.Two
                    }
                    .Select(v => new Card(v))
                    .ToArray();

                var pokerHand = new PokerHand(card1, card2);

                var bestCard = pokerHand.GetHighestCard();
                var bestExpectedCard = cardsByValuesDescending.First(c => c == card1 || c == card2);

                var lowestCard = pokerHand.GetLowestCard();
                var lowestExpectedCard = cardsByValuesDescending.Last(c => c == card1 || c == card2);

                return (bestExpectedCard == bestCard).Label($"The best card is {bestCard} on hand {pokerHand}")
                   .And(lowestExpectedCard == lowestCard).Label($"The lowest card is {lowestCard} on hand {pokerHand}");
            })
            .QuickCheckThrowOnFailure();

        [Property]
        public void LowestPairIsAlwaysBetterThanRandomHand()
            => Prop.ForAll<Card, Card>((card1, card2) => new Func<bool>(() =>
            {
                var randomHand = new PokerHand(card1, card2);
                var lowestPair = new PokerHand(Two, Two);

                var pairAsLeftHand = PokerHand.GetBestHand(lowestPair, randomHand);
                var pairAsRightHand = PokerHand.GetBestHand(randomHand, lowestPair);

                var expectedWiningHand = Maybe<PokerHand>.Just(lowestPair);
                return pairAsLeftHand == expectedWiningHand && pairAsRightHand == expectedWiningHand;
            })
            .When(card1 != card2))
            .QuickCheckThrowOnFailure();
    }
}
