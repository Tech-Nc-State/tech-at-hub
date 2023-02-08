using System;
using System.Collections.Generic;

namespace Tech_HubAPI.Models
{
    public class Issue
    {
        public Issue(int id, string title, User author, Repository repository, string activity, string buildInfo, string summary,string stepsToReproduce,
            string expectedResults, string actualResults)
        {
            Id = id;
            Title = title;
            Author = author;
            Repository = repository;
            Activity = activity;
            BuildInfo = buildInfo;
            Summary = summary;
            StepsToReproduce = stepsToReproduce;
            ExpectedResults = expectedResults;
            ActualResults = actualResults;
            Comments = new List<Comment>();
            Assignees = new List<User>();
            Labels = new List<Label>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public User Author { get; set; }

        public Repository Repository { get; set; }

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

