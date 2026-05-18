using System;
using System.Collections.Generic;
using Arcanum.Data;

namespace Arcanum.Reading
{
    public static class TodayCardService
    {
        public static TarotCardDefinition GetTodayCard()
        {
            return GetCardForDate(DateTime.Today);
        }

        public static TarotCardDefinition GetCardForDate(DateTime date)
        {
            var cards = PrototypeTarotDeck.Cards;
            if (cards.Count == 0)
            {
                throw new InvalidOperationException("Prototype tarot deck must contain at least one card.");
            }

            var index = GetDateIndex(date, cards.Count);
            return PrototypeTarotDeck.GetAt(index);
        }

        public static IReadOnlyList<TarotCardDefinition> GetSpreadForDate(DateTime date)
        {
            var cards = PrototypeTarotDeck.Cards;
            if (cards.Count == 0)
            {
                throw new InvalidOperationException("Prototype tarot deck must contain at least one card.");
            }

            var spread = new TarotCardDefinition[cards.Count];
            for (var i = 0; i < cards.Count; i++)
            {
                spread[i] = cards[i];
            }

            var seed = GetDateSeed(date);
            for (var i = spread.Length - 1; i > 0; i--)
            {
                seed = NextSeed(seed);
                var swapIndex = (int)(seed % (uint)(i + 1));
                (spread[i], spread[swapIndex]) = (spread[swapIndex], spread[i]);
            }

            return spread;
        }

        public static int GetDateIndex(DateTime date, int cardCount)
        {
            if (cardCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cardCount), cardCount, "Card count must be greater than zero.");
            }

            return (int)(GetDateSeed(date) % (uint)cardCount);
        }

        private static uint GetDateSeed(DateTime date)
        {
            var localDate = date.Date;
            var dayNumber = localDate.Year * 10000 + localDate.Month * 100 + localDate.Day;
            return unchecked((uint)dayNumber * 2654435761u);
        }

        private static uint NextSeed(uint seed)
        {
            return unchecked(seed * 1664525u + 1013904223u);
        }
    }
}
