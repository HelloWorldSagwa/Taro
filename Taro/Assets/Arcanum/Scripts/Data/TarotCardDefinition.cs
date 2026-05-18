using System;
using UnityEngine;

namespace Arcanum.Data
{
    [Serializable]
    public sealed class TarotCardDefinition
    {
        [SerializeField] private string cardId;
        [SerializeField] private string displayName;
        [SerializeField] private int number;
        [SerializeField] private Color accentColor;
        [SerializeField] private string[] keywords;
        [SerializeField] private string masterLine;
        [SerializeField] private string todayMessage;

        public TarotCardDefinition(
            string cardId,
            string displayName,
            int number,
            Color accentColor,
            string[] keywords,
            string masterLine,
            string todayMessage)
        {
            this.cardId = cardId;
            this.displayName = displayName;
            this.number = number;
            this.accentColor = accentColor;
            this.keywords = keywords;
            this.masterLine = masterLine;
            this.todayMessage = todayMessage;
        }

        public string CardId => cardId;
        public string DisplayName => displayName;
        public int Number => number;
        public Color AccentColor => accentColor;
        public string[] Keywords => keywords;
        public string MasterLine => masterLine;
        public string TodayMessage => todayMessage;
    }
}
