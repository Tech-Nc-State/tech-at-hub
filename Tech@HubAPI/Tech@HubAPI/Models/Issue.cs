using System;
using System.Collections.Generic;

namespace Tech_HubAPI.Models
{
    public class Issue
    {
        public Issue(string id, string title, User author, string activity, string buildInfo, string summary,string stepsToReproduce,
            string expectedResults, string actualResults, List<Comment> comments, List<User> assignees, List<Label> labels)
        {
            Id = id;
            Title = title;
            Author = author;
            Activity = activity;
            BuildInfo = buildInfo;
            Summary = summary;
            StepsToReproduce = stepsToReproduce;
            ExpectedResults = expectedResults;
            ActualResults = actualResults;
            Comments = comments;
            Assignees = assignees;
            Labels = labels;
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public User Author { get; set; }

        public string Activity { get; set; }

        public string BuildInfo { get; set; }

        public string Summary { get; set; }

        public string StepsToReproduce { get; set; }

        public string ExpectedResults { get; set; }

        public string ActualResults { get; set; }

        public List<Comment> Comments { get; set; }

        public List<User> Assignees { get; set; }

        public List<Label> Labels { get; set; }
    }
}

