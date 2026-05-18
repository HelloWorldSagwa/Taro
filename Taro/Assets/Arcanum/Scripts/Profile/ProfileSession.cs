using System;
using Arcanum.Reading;
using UnityEngine;

namespace Arcanum.Profile
{
    public enum ProfileArchetype
    {
        FirstVisit,
        LoveConcern,
        CareerDecision
    }

    [Serializable]
    public sealed class ProfileData
    {
        public ProfileData(string displayName, string birthMonth, string focus)
        {
            DisplayName = displayName;
            BirthMonth = birthMonth;
            Focus = focus;
        }

        public string DisplayName { get; }
        public string BirthMonth { get; }
        public string Focus { get; }

        public string SafeDisplayName => string.IsNullOrWhiteSpace(DisplayName) ? Korean.Guest : DisplayName.Trim();
        public string SafeBirthMonth => string.IsNullOrWhiteSpace(BirthMonth) ? Korean.NotProvided : BirthMonth.Trim();
        public string SafeFocus => string.IsNullOrWhiteSpace(Focus) ? Korean.TodayFlow : Focus.Trim();
    }

    public static class ProfileSession
    {
        private const string NameKey = "Arcanum.Profile.DisplayName";
        private const string BirthMonthKey = "Arcanum.Profile.BirthMonth";
        private const string FocusKey = "Arcanum.Profile.Focus";

        private static ProfileData current;

        public static ProfileData Current
        {
            get
            {
                if (current != null)
                {
                    return current;
                }

                return TryLoad(out current) ? current : null;
            }
        }

        public static string DisplayName => Current?.SafeDisplayName ?? Korean.Guest;
        public static bool HasProfile => Current != null;

        public static ProfileData Create(string displayName, string birthMonth, string focus)
        {
            var profile = new ProfileData(
                string.IsNullOrWhiteSpace(displayName) ? Korean.Guest : displayName.Trim(),
                string.IsNullOrWhiteSpace(birthMonth) ? Korean.NotProvided : birthMonth.Trim(),
                string.IsNullOrWhiteSpace(focus) ? Korean.TodayFlow : focus.Trim());

            Save(profile);
            return profile;
        }

        public static void Select(ProfileArchetype archetype)
        {
            switch (archetype)
            {
                case ProfileArchetype.LoveConcern:
                    Create(Korean.LoveVisitor, Korean.NotProvided, Korean.RelationshipFocus);
                    break;
                case ProfileArchetype.CareerDecision:
                    Create(Korean.CareerVisitor, Korean.NotProvided, Korean.WorkChoiceFocus);
                    break;
                default:
                    Create(Korean.FirstVisitor, Korean.NotProvided, Korean.TodayFlow);
                    break;
            }
        }

        public static string GetFocusLabel()
        {
            return Current?.SafeFocus ?? Korean.TodayFlow;
        }

        public static void Save(ProfileData profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            current = profile;
            PlayerPrefs.SetString(NameKey, profile.SafeDisplayName);
            PlayerPrefs.SetString(BirthMonthKey, profile.SafeBirthMonth);
            PlayerPrefs.SetString(FocusKey, profile.SafeFocus);
            PlayerPrefs.Save();
        }

        public static void Clear()
        {
            current = null;
            PlayerPrefs.DeleteKey(NameKey);
            PlayerPrefs.DeleteKey(BirthMonthKey);
            PlayerPrefs.DeleteKey(FocusKey);
            PlayerPrefs.Save();
            ReadingSession.Clear();
        }

        private static bool TryLoad(out ProfileData profile)
        {
            if (!PlayerPrefs.HasKey(NameKey))
            {
                profile = null;
                return false;
            }

            profile = new ProfileData(
                PlayerPrefs.GetString(NameKey, Korean.Guest),
                PlayerPrefs.GetString(BirthMonthKey, Korean.NotProvided),
                PlayerPrefs.GetString(FocusKey, Korean.TodayFlow));
            return true;
        }
    }

    public static class Korean
    {
        public const string Guest = "손님";
        public const string NotProvided = "미입력";
        public const string TodayFlow = "오늘의 흐름";
        public const string FirstVisitor = "처음 온 손님";
        public const string LoveVisitor = "마음이 궁금한 손님";
        public const string CareerVisitor = "갈림길 앞의 손님";
        public const string RelationshipFocus = "관계와 마음";
        public const string WorkChoiceFocus = "일과 선택";
    }
}
