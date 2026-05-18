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
                "\uBC14\uBCF4",
                0,
                new Color(0.86f, 0.78f, 0.42f),
                new[] { "\uC2DC\uC791", "\uAC00\uBCBC\uC6C0", "\uB3C4\uC57D" },
                "\uCCAB\uAC78\uC74C\uC740 \uC774\uBBF8 \uB2F9\uC2E0 \uBC1C\uBC11\uC5D0\uC11C \uBE5B\uB098\uACE0 \uC788\uC5B4\uC694.",
                "\uC624\uB298\uC740 \uC0B0\uB73B\uD55C \uC2DC\uC791\uC774 \uBCF4\uC0C1\uC744 \uC90D\uB2C8\uB2E4. \uD55C \uAC00\uC9C0 \uC120\uD0DD\uC744 \uAC00\uBCCD\uAC8C \uB2E4\uB8E8\uB418 \uD750\uB9BF\uD558\uAC8C \uB0A8\uAE30\uC9C0\uB294 \uB9C8\uC138\uC694."),
            new TarotCardDefinition(
                "major_01_magician",
                "\uB9C8\uBC95\uC0AC",
                1,
                new Color(0.58f, 0.35f, 0.92f),
                new[] { "\uC9D1\uC911", "\uAE30\uC220", "\uC2E0\uD638" },
                "\uB2F9\uC2E0\uC740 \uC9C0\uAE08 \uC4F0\uB294 \uAC83\uBCF4\uB2E4 \uB354 \uB9CE\uC740 \uB3C4\uAD6C\uB97C \uC774\uBBF8 \uAC00\uC9C0\uACE0 \uC788\uC5B4\uC694.",
                "\uAD6C\uCCB4\uC801\uC778 \uD589\uB3D9 \uD558\uB098\uB97C \uACE8\uB77C \uB208\uC5D0 \uBCF4\uC774\uAC8C \uB9CC\uB4DC\uC138\uC694. \uBD84\uBA85\uD55C \uC2E0\uD638 \uB4A4\uC5D0 \uCD94\uC9C4\uB825\uC774 \uB530\uB77C\uC635\uB2C8\uB2E4."),
            new TarotCardDefinition(
                "major_06_lovers",
                "\uC5F0\uC778",
                6,
                new Color(0.92f, 0.33f, 0.56f),
                new[] { "\uC120\uD0DD", "\uC5F0\uACB0", "\uC9C4\uC2EC" },
                "\uAC10\uC815\uC740 \uC194\uC9C1\uD574\uC9C8 \uB54C\uC5D0\uC57C \uC4F8\uBAA8 \uC788\uB294 \uB2E8\uC11C\uAC00 \uB429\uB2C8\uB2E4.",
                "\uD655\uC2E0\uB9CC \uC887\uC9C0 \uB9C8\uC138\uC694. \uC695\uB9DD\uACFC \uD488\uC704\uB97C \uD568\uAED8 \uC9C0\uD0A4\uB294 \uC120\uD0DD\uC774 \uBB34\uC5C7\uC778\uC9C0 \uBB3C\uC5B4\uBCF4\uC138\uC694."),
            new TarotCardDefinition(
                "major_13_death",
                "\uC8FD\uC74C",
                13,
                new Color(0.48f, 0.54f, 0.66f),
                new[] { "\uB05D\uB9FA\uC74C", "\uBCC0\uD654", "\uB193\uC544\uC8FC\uAE30" },
                "\uBB38\uC774 \uCF85 \uB2EB\uD788\uB294 \uAC8C \uC544\uB2C8\uC5D0\uC694. \uB5A0\uB0A0 \uB9CC\uD07C \uBC29\uC774 \uC870\uC6A9\uD574\uC9C4 \uAC81\uB2C8\uB2E4.",
                "\uB0A1\uC740 \uD328\uD134 \uD558\uB098\uB97C \uC624\uB298 \uB05D\uB0B4\uC138\uC694. \uC791\uACE0 \uBD84\uBA85\uD558\uACE0 \uC2E4\uC81C\uC801\uC778 \uB9C8\uBB34\uB9AC\uBA74 \uCDA9\uBD84\uD569\uB2C8\uB2E4."),
            new TarotCardDefinition(
                "major_18_moon",
                "\uB2EC",
                18,
                new Color(0.33f, 0.58f, 0.86f),
                new[] { "\uAFC8", "\uC548\uAC1C", "\uC9C1\uAC10" },
                "\uBAA8\uB4E0 \uADF8\uB9BC\uC790\uAC00 \uACBD\uACE0\uB294 \uC544\uB2C8\uC5D0\uC694. \uC5B4\uB5A4 \uAC83\uC740 \uC544\uC9C1 \uBAA8\uB974\uB294 \uC815\uBCF4\uC77C \uBFD0\uC785\uB2C8\uB2E4.",
                "\uADF9\uC801\uC778 \uACB0\uB860\uC740 \uBBF8\uB8E8\uC138\uC694. \uB450\uB824\uC6C0\uC744 \uBBFF\uAE30 \uC804\uC5D0 \uC0AC\uC2E4\uBD80\uD130 \uC801\uC5B4\uBCF4\uC138\uC694."),
            new TarotCardDefinition(
                "major_21_world",
                "\uC138\uACC4",
                21,
                new Color(0.35f, 0.78f, 0.66f),
                new[] { "\uC644\uC131", "\uBCF4\uC0C1", "\uB3C4\uCC29" },
                "\uBB34\uC5B8\uAC00\uAC00 \uD55C \uBC14\uD034\uB97C \uB3CC\uC544 \uC81C\uC790\uB9AC\uB85C \uC654\uC5B4\uC694. \uADF8 \uBAA8\uC591\uC744 \uC54C\uC544\uCC28\uB824\uB3C4 \uB429\uB2C8\uB2E4.",
                "\uC624\uB298 \uD558\uB098\uC758 \uACE0\uB9AC\uB97C \uB2EB\uC73C\uC138\uC694. \uB610 \uB2E4\uB978 \uC2DC\uC791\uBCF4\uB2E4 \uC644\uB8CC\uAC00 \uB354 \uD070 \uC5D0\uB108\uC9C0\uB97C \uC904 \uAC81\uB2C8\uB2E4.")
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
