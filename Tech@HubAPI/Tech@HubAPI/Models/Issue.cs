using System;
using System.Collections.Generic;

namespace Tech_HubAPI.Models
{
    public class Issue
    {
        public Issue(int id, string issueTitle, int authorId, int repositoryId, string activity, string buildInfo, string summary, string stepsToReproduce,
            string expectedResults, string actualResults)
        {
            Id = id;
            IssueTitle = issueTitle;
            AuthorId = authorId;
            RepositoryId = repositoryId;
            Activity = activity;
            BuildInfo = buildInfo;
            Summary = summary;
            StepsToReproduce = stepsToReproduce;
            ExpectedResults = expectedResults;
            ActualResults = actualResults;
            Comments = new List<string>();
            Assignees = new List<string>();
            Labels = new List<string>();
        }

        public int Id { get; set; }

        public string IssueTitle { get; set; }

        public int AuthorId { get; set; }

        public int RepositoryId { get; set; }

        public string Activity { get; set; }

        public string BuildInfo { get; set; }

        public string Summary { get; set; }

        public string StepsToReproduce { get; set; }

        public string ExpectedResults { get; set; }

        public string ActualResults { get; set; }

        public List<string> Comments { get; set; }

        public List<string> Assignees { get; set; }

        public List<string> Labels { get; set; }
    }
}

