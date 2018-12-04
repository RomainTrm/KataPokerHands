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
        public bool TwoIsTheWorstCard(Values value)
        {
            var card = new Card(value);
            var handLeft = new PokerHand(Two, card);
            var handRight = new PokerHand(card, Two);

            return handLeft.GetHighestCard() == card 
                && handRight.GetHighestCard() == card
                && handLeft.GetLowestCard() == Two
                && handRight.GetLowestCard() == Two;
        }
    }
}
