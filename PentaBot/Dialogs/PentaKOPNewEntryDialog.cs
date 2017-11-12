using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PentaBot.Dialogs
{
    [Serializable]
    public class PentaKOPNewEntry
    {
        [Prompt("How many hours have you worked?")]
        public int NumberOfHours { get; set; }
        [Prompt("What project was being done?")]
        public string Project { get; set; }
        //public Project Project { get; set; }
        [Prompt("Which kind of activity?")]
        public string ActivityType { get; set; }
        [Prompt("Please provide a comment")]
        public string Comment { get; set; }
    }
    public static class PentaKOPNewEntryDialog
    { 
        public static IDialog<PentaKOPNewEntry> CreateDialog(int hours = 0, string project = null, string activityType = null, string description = null)
        {
            return new FormDialog<PentaKOPNewEntry>(new PentaKOPNewEntry { NumberOfHours = hours, Project = project, ActivityType = activityType }, BuildForm, FormOptions.PromptInStart);
        }

        private static IForm<PentaKOPNewEntry> BuildForm()
        {
            return new FormBuilder<PentaKOPNewEntry>()
                .Field(nameof(PentaKOPNewEntry.Project), validate: async (state, value) =>
                {
                    var projectName = (string)value;
                    var possibleProjects = new List<string>() { "Project 1", "Project 2", "Project 3", "Project 4", "Project 5", "Project 6", "Project 7", "Project 8" };

                    string exactMatch = possibleProjects.FirstOrDefault(b => b.Equals(projectName, StringComparison.CurrentCultureIgnoreCase));
                    if (exactMatch != null)
                    {
                        return new ValidateResult { IsValid = true, Value = possibleProjects[0] };
                    }

                    switch (possibleProjects.Count)
                    {
                        case 0:
                            return new ValidateResult { IsValid = false, Feedback = "Don't know such project... Try again." };
                        case 1:
                            return new ValidateResult { IsValid = true, Value = possibleProjects[0] };
                    }
                    return new ValidateResult
                    {
                        IsValid = false,
                        Feedback = "I'm not sure which one",
                        Choices = possibleProjects.Select(b => new Choice
                        {
                            Value = b,
                            Description = new DescribeAttribute(b),
                            Terms = new TermsAttribute(b)
                        })
                    };
                }).Field(nameof(PentaKOPNewEntry.ActivityType), validate: async (state, value) =>
                {
                    var activityType = (string)value;
                    var possibleActivities = new List<string>() { "Programistyczne", "Analityczne", "Organizacyjne", "Projektowe", "Wdrożenie", "Konferencja", "Gaszenie pożaru"};

                    string exactMatch = possibleActivities.FirstOrDefault(b => b.Equals(activityType, StringComparison.CurrentCultureIgnoreCase));
                    if (exactMatch != null)
                    {
                        return new ValidateResult { IsValid = true, Value = possibleActivities[0] };
                    }

                    switch (possibleActivities.Count)
                    {
                        case 0:
                            return new ValidateResult { IsValid = false, Feedback = "Don't know such project... Try again." };
                        case 1:
                            return new ValidateResult { IsValid = true, Value = possibleActivities[0] };
                    }
                    return new ValidateResult
                    {
                        IsValid = false,
                        Feedback = "I'm not sure which one",
                        Choices = possibleActivities.Select(b => new Choice
                        {
                            Value = b,
                            Description = new DescribeAttribute(b),
                            Terms = new TermsAttribute(b)
                        })
                    };
                })
                .AddRemainingFields()
                .Confirm("Just making sure, is this what you wanted? {*}")
                .Build();
        }
    }
}
