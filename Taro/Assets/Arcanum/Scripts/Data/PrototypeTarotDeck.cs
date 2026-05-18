using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arcanum.Data
{
    public static class PrototypeTarotDeck
    {
        private static readonly TarotCardDefinition[] cards =
        {
            new TarotCardDefinition(
                "major_00_fool",
                "The Fool",
                0,
                new Color(0.86f, 0.78f, 0.42f),
                new[] { "start", "lightness", "leap" },
                "The first step is already glowing under your feet.",
                "Today rewards a clean start. Keep one choice playful, but do not leave it vague."),
            new TarotCardDefinition(
                "major_01_magician",
                "The Magician",
                1,
                new Color(0.58f, 0.35f, 0.92f),
                new[] { "focus", "skill", "signal" },
                "You have more tools on this table than you are using.",
                "Pick one concrete action and make it visible. Momentum follows a clear signal."),
            new TarotCardDefinition(
                "major_06_lovers",
                "The Lovers",
                6,
                new Color(0.92f, 0.33f, 0.56f),
                new[] { "choice", "bond", "truth" },
                "A feeling becomes useful only when it is honest.",
                "Do not chase certainty. Ask which choice lets you respect both desire and dignity."),
            new TarotCardDefinition(
                "major_13_death",
                "Death",
                13,
                new Color(0.48f, 0.54f, 0.66f),
                new[] { "ending", "change", "release" },
                "This is not a door slamming. It is a room becoming quiet enough to leave.",
                "Let one stale pattern end today. Make the ending small, deliberate, and real."),
            new TarotCardDefinition(
                "major_18_moon",
                "The Moon",
                18,
                new Color(0.33f, 0.58f, 0.86f),
                new[] { "dream", "fog", "intuition" },
                "Not every shadow is a warning. Some are only missing information.",
                "Delay dramatic conclusions. Write down the facts before trusting the fear."),
            new TarotCardDefinition(
                "major_21_world",
                "The World",
                21,
                new Color(0.35f, 0.78f, 0.66f),
                new[] { "closure", "reward", "arrival" },
                "Something has come full circle. Let yourself notice the shape of it.",
                "Finish one loop today. Completion will give you more energy than another new tab.")
        };

        public static IReadOnlyList<TarotCardDefinition> Cards => cards;

        public static bool TryGetCard(string cardId, out TarotCardDefinition card)
        {
            if (string.IsNullOrEmpty(cardId))
            {
                card = null;
                return false;
            }

            for (var i = 0; i < cards.Length; i++)
            {
                if (string.Equals(cards[i].CardId, cardId, StringComparison.Ordinal))
                {
                    card = cards[i];
                    return true;
                }
            }

            card = null;
            return false;
        }

        public static TarotCardDefinition GetAt(int index)
        {
            if (index < 0 || index >= cards.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), index, "Card index is outside the prototype deck.");
            }

            return cards[index];
        }
    }
}
