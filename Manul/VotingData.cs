using System;
using System.Collections.Generic;
using System.Globalization;

namespace Manul
{
    public class VotingData
    {
        private const int DefaultDurationInMinutes = 1440;    // 24 часа.
        public string Name { get; set; }
        public string Theme { get; set; }
        public string Author { get; set; }
        public string CreationDate { get; set; }
        public string ExpiryDate { get; set; }
        public Dictionary<int, string> Answers { get; set; } 
        public Dictionary<string, List<int>> UserAnswers { get; set; }

        public VotingData(string name, string theme, Dictionary<int, string> answers, string author = "",
                string creationDate = "", string expiryDate = "", Dictionary<string, List<int>> userAnswers = null)
        {
            Name = name;
            Theme = theme;
            Author = string.IsNullOrEmpty(author) || string.IsNullOrWhiteSpace(author) ? "Аноним" : author;
            CreationDate = string.IsNullOrEmpty(creationDate) || string.IsNullOrWhiteSpace(creationDate)
                    ? DateTime.Now.ToString(CultureInfo.InvariantCulture) : creationDate;
            ExpiryDate = string.IsNullOrEmpty(expiryDate) || string.IsNullOrWhiteSpace(expiryDate)
                    ? DateTime.Parse(CreationDate, CultureInfo.InvariantCulture).AddMinutes(DefaultDurationInMinutes)
                    .ToString(CultureInfo.InvariantCulture) : expiryDate;
            Answers = answers ?? new Dictionary<int, string>();
            UserAnswers = userAnswers ?? new Dictionary<string, List<int>>();
        }
    }
}