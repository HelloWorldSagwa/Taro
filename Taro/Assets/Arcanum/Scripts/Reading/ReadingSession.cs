using System;
using Arcanum.Data;

namespace Arcanum.Reading
{
    public static class ReadingSession
    {
        private static TarotCardDefinition selectedCard;
        private static string selectedCardId;

        public static TarotCardDefinition SelectedCard
        {
            get
            {
                if (selectedCard != null)
                {
                    return selectedCard;
                }

                if (PrototypeTarotDeck.TryGetCard(selectedCardId, out var restoredCard))
                {
                    selectedCard = restoredCard;
                    return selectedCard;
                }

                return null;
            }
        }

        public static string SelectedCardId => selectedCardId;
        public static bool HasSelection => SelectedCard != null;

        public static void Select(TarotCardDefinition card)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card));
            }

            selectedCard = card;
            selectedCardId = card.CardId;
        }

        public static bool SelectById(string cardId)
        {
            if (!PrototypeTarotDeck.TryGetCard(cardId, out var card))
            {
                return false;
            }

            Select(card);
            return true;
        }

        public static TarotCardDefinition EnsureSelectionOrTodayCard()
        {
            if (SelectedCard != null)
            {
                return selectedCard;
            }

            var todayCard = TodayCardService.GetTodayCard();
            Select(todayCard);
            return todayCard;
        }

        public static void Clear()
        {
            selectedCard = null;
            selectedCardId = null;
        }
    }
}
