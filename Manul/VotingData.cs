namespace Manul;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

public class VotingData
{
    private const int DefaultDurationInMinutes = 1440;    // 24 часа.
    private static int _index;
    public string Name { get; set; }
    public string Theme { get; set; }
    public string Author { get; set; }
    public string CreationDate { get; set; }
    public string ExpiryDate { get; set; }
    public Dictionary<int, string> Answers { get; set; } 
    public Dictionary<string, List<int>> UserAnswers { get; set; }

    public VotingData(string name, string theme, Dictionary<int, string> answers, string author = "",
            string creationDate = "", string expiryDate = "", Dictionary<string, List<int>> userAnswers = null,
            int durationInMinutes = DefaultDurationInMinutes)
    {
        Name = string.IsNullOrEmpty(name.Trim()) ? $"Unnamed_{_index++}" : name.Trim();
        Theme = theme.Trim();
        Author = author.Trim();
        CreationDate = string.IsNullOrEmpty(creationDate.Trim())
                ? DateTime.Now.ToString(CultureInfo.CurrentCulture)
                : creationDate.Trim();
        ExpiryDate = string.IsNullOrEmpty(expiryDate.Trim())
                ? DateTime.Parse(CreationDate, CultureInfo.CurrentCulture).AddMinutes(durationInMinutes > 0
                        ? durationInMinutes : DefaultDurationInMinutes).ToString(CultureInfo.CurrentCulture)
                : expiryDate.Trim();
        Answers = answers ?? new Dictionary<int, string>();
        UserAnswers = userAnswers ?? new Dictionary<string, List<int>>();
    }

    public override string ToString()
    {
        var result = new StringBuilder(string.Format(CultureInfo.InvariantCulture, $"Voting data:\n\tName: {Name}"
                + $"\n\tTheme: {Theme}\n\tAuthor: {Author}\n\tCreation date: {CreationDate}\n\tExpiry date:"
                + $" {ExpiryDate}\n\tAnswers:\n\t\t"));

        foreach (var (index, answer) in Answers)
        {
            result.Append(string.Format(CultureInfo.InvariantCulture, $"{index}) {answer}\n\t\t"));
        }

        result.Remove(result.Length - 1, 1);    // Удаление лишней табуляции.
        result.Append(string.Format(CultureInfo.InvariantCulture, "Users answers:\n\t\t"));
            
        foreach (var (username, answers) in UserAnswers)
        {
            result.Append(string.Format(CultureInfo.InvariantCulture, $"{username}: "
                    + $"{answers.Aggregate(string.Empty, (current, answer) => current + $"{answer}, ")}"));
            result.Remove(result.Length - 2, 2);    // Удаление запятой и пробела, находящихся после последнего варианта ответа.
            result.Append(string.Format(CultureInfo.InvariantCulture, "\n\t\t"));
        }

        return result.ToString();
    }
}