module PokerHand_PropertyBase

open FsCheck
open FsCheck.Xunit

type Card = Two | Three | Four | Five | Six | Seven | Eight | Nine | Ten | Jack | Queen | King | Ace
type Result = Winner of (Card * Card) | Draw

let cardValue = function
    | Ace -> 14
    | King -> 13
    | Queen -> 12
    | Jack -> 11
    | Ten -> 10
    | Nine -> 9
    | Eight -> 8
    | Seven -> 7
    | Six -> 6
    | Five -> 5
    | Four -> 4
    | Three -> 3
    | Two -> 2

let getBestCard card1 card2 = 
    [card1; card2]
    |> List.sortByDescending cardValue
    |> List.head
    
let getWorstCard card1 card2 = 
    [card1; card2]
    |> List.sortBy cardValue
    |> List.head
    
let getBestPair card1 card2 = 
    if card1 = card2 then 
        Draw
    else
        let bestCard = getBestCard card1 card2
        Winner (bestCard, bestCard)

let getBestHandWhenNotPairs (h1c1, h1c2) (h2c1, h2c2) =
    let compareHands getCardToCompare =
        let cardHand1 = getCardToCompare h1c1 h1c2
        let cardHand2 = getCardToCompare h2c1 h2c2

        if cardHand1 = cardHand2
        then Draw
        elif getBestCard cardHand1 cardHand2 = cardHand1 
        then Winner (h1c1, h1c2)
        else Winner (h2c1, h2c2)

    let comparePerHandsBestsCards = compareHands getBestCard
    let comparePerHandsWorstCards = compareHands getWorstCard

    match comparePerHandsBestsCards, comparePerHandsWorstCards with
    | Draw, Draw -> Draw
    | Draw, winner -> winner
    | winner, _ -> winner

let (|Pair|_|) (card1, card2) =
    if card1 = card2
    then Some card1
    else None

let getBestHand hand1 hand2 = 
    match hand1, hand2 with
    | Pair card1, Pair card2 -> getBestPair card1 card2
    | Pair _, _ -> Winner hand1
    | _, Pair _ -> Winner hand2
    | _, _ -> getBestHandWhenNotPairs hand1 hand2

// Tests for getting best or worst card

[<Property>]
let ``Ace is the best card`` card = 
    ((getBestCard Ace card = Ace) && (getBestCard card Ace = Ace))     |@ "Ace is the best card" .&. 
    ((getWorstCard card Ace = card) && (getWorstCard Ace card = card)) |@ "The other card is the worst card"
    
[<Property>]
let ``Two is the worst card`` card = 
    ((getWorstCard Two card = Two) && (getWorstCard card Two = Two)) |@ "Two is the worst card" .&.
    ((getBestCard card Two = card) && (getBestCard Two card = card)) |@ "The other card is the best card"

[<Property>]
let ``Each card has its specific value`` card1 card2 =
    card1 <> card2 ==> lazy
    let cardsValuesDescending = [Ace; King; Queen; Jack; Ten; Nine; Eight; Seven; Six; Five; Four; Three; Two]
    let bestCard = cardsValuesDescending 
                   |> List.where (fun card -> card = card1 || card = card2) 
                   |> List.head
    (bestCard = getBestCard card1 card2)   |@ "Identify best card" .&.
    (bestCard <> getWorstCard card1 card2) |@ "Identify worst card"

// Tests for getting the best hand

[<Property>]
let ``The best hand is the one with the strongest card`` card1 card2 =
    (card1 <> card2 && card1 <> Two && card2 <> Two) ==> lazy
    let bestCard = getBestCard card1 card2
    (getBestHand (card1, Two) (card2, Two) = Winner (bestCard, Two)) |@ "Pair with best card as left card" .&.
    (getBestHand (Two, card1) (Two, card2) = Winner (Two, bestCard)) |@ "Pair with best card as right card"

[<Property>]
let ``When the strongest cards are equals, the best hand is the one with the strongest second card`` card1 card2 =
    (card1 <> card2 && card1 <> Ace && card2 <> Ace) ==> lazy
    let bestSecondCard = getBestCard card1 card2
    (getBestHand (card1, Ace) (card2, Ace) = Winner (bestSecondCard, Ace)) |@ "Pair with best card as left card" .&.
    (getBestHand (Ace, card1) (Ace, card2) = Winner (Ace, bestSecondCard)) |@ "Pair with best card as right card"
    
[<Property>]
let ``Pair is always better than random hand`` card1 card2 =
    card1 <> card2 ==> lazy
    (getBestHand (card1, card1) (card1, card2) = Winner (card1, card1)) |@ "Pair as left hand" .&.
    (getBestHand (card1, card2) (card2, card2) = Winner (card2, card2)) |@ "Pair as right hand"

[<Property>]
let ``The best pair is the one with best card`` card1 card2 =
    card1 <> card2 ==> lazy
    let bestCard = getBestCard card1 card2
    getBestHand (card1, card1) (card2, card2) = Winner (bestCard, bestCard)

[<Property>]
let ``Return draw when hands are equals`` card1 card2 =
    getBestHand (card1, card2) (card2, card1) = Draw