<p align="center">
  <img src="https://github.com/Tech-Nc-State/tech-at-hub/blob/main/frontend/src/assets/logo.svg">
</p>

# tech-at-hub

The average student in Computer Science, when first starting out, *has no idea what version control is*, or how to manage projects. Instead of worrying about the technicalities and processes associated with development on GitHub, Tech@Hub seeks to **minimize that overhead**. The goal is for that the beginner programmer to spend less time on project management, and more time learning to program. Tech@Hub is intended to be a platform with similar purpose to GitHub, but with a **lowered barrier of entry**.  It will provide an open space for developers from all backgrounds and skill levels to collaborate on projects, all while *keeping the focus on the code*.  

**Tech@Hub** is a code hosting web project created by students at **Tech@NCState**. Created for the purpose of sharing projects between students and other individuals, Tech@Hub is a great way to *showcase different coding projects*. This Version Control Code Hoster is a full-stack application, complete with Node.js, React, and a .NET framework. Having been in *development within the club for over 3 years*, Tech@Hub has plenty of interesting development features described below.

## What is a Version Control?

Version control is a system that helps manage changes made to code or documents over time. It allows multiple developers to work on the same project simultaneously, without stepping all over each other's modifications. This can be helpful in all sorts of project and group-based environments. Whether working on a massive project with hundreds of contributors (a fancy word for anyone who has added something to the project), a class project with a partner, or even just a solo development project, integrating version control helps to ease the workflow greatly.

Version control sounds great, but how does it all work? There are two main types of version control systems, centralized and distributed. Distributed is a very popular option, and its what we used for Tech@Hub. In a distributed version control system, each user has a complete copy of the entire repository (another fancy word for project and all related files). This includes the history, a report of each change made to the project over time. This facilitates a way for users to work independently, and share changes between repositories. Git is the version control system we use. GitHub also uses Git- yes, Git and GitHub are different. Git is the distributed version control system that manages repositories, GitHub is a web application overlay built for centralizing Git projects to a single platform. Tech@Hub is acting as GitHub, utilizing Git to create an easy interface when using Git.

Git Slang can be pretty confusing when starting out, so lets break down some of the common terms. We've already talked about a repository, the grouping of files making up your project. The big tool of version control is what you can do with this repository. What makes a repository great is that changes can be shared. To share changes you make, you will *push* your local changes made on your machine. To get the most up-to-date changes on the project, you *pull* them from the repository. An extremely common flow of what can be done with a repository can be seen in our developer's guide wiki page [here](https://github.com/Tech-Nc-State/tech-at-hub/wiki/Developer's-Guide)!

## Technologies Used

Here is a sneak peak at some of our tech-stack. A more in-depth  explanation can be found in our architecture wiki page [here](https://github.com/Tech-Nc-State/tech-at-hub/wiki/Architecture).

1. **C# Backend**  
    Used for our backend logic and REST API to map web requests from the client side to database queries and vice versa.
2. **Swagger**  
    Tool used for documenting and managing our REST API.
3. **HTML, CSS, Typescript**  
    Frontend languages utilized in building out our website's user interface.
4. **.NET**  
    Provides a runtime environment for executing and managing applications.




## Installation & Environment Setup

We use pre-commit to automatically manage our git hooks. pre-commit is required to develop in this project. We enforce a strict LF-only line ending policy. Our Dockerfiles will break if any CRLF's are introduced into the project. One of the pre-commit hooks is a LF enforcer hook. To setup:

1. Run `pip install pre-commit`
2. Run `pre-commit install`

In addition, to prevent git from auto-converting LF's to CRLF's on Windows machines, run

```bash
git config core.autocrlf false
```

## Project Configuration & Getting Started

```json
{
    "ConnectionStrings": {
        "MySqlDatabase": "server=localhost; port=3306; uid=root; password=dbpass; database=tech-at-hub;"
    },
    "JWT": {
        "SecretKey": "blahblahblahblah"
    },
    "Environment": {
        "Platform": "Windows",
        "GitPath": "C:\\Program Files\\Git\\bin\\",
        "BinPath": "C:\\Program Files\\Git\\usr\\bin\\",
        "DefaultWorkingDirectory": "C:\\Users\\jpbre\\Documents\\NCSU\\tech-at-hub\\"
    }
}
```

- ```JWT:SecretKey``` - Can be any string of at least 16 characters
- ```Environment:GitPath``` - Path to your Git binary projects
- ```Environment:BinPath``` - Path to your platform common executables
- ```Environment:DefaultWorkingDirectory``` - Path to your root ```tech_at_hub``` folder

In-depth developer's guide can be found on our wiki page.

## Contributors

1. Joey Bream
2. Matthew Martin
3. Michael Costello
4. Neil Bennett
5. Ziyad Abdelaziz
6. Aarnav Chhatrala
7. Jack Maggio
8. Hrishi Batta
9. Aditya Konidena
10. Roman Peace
11. Shrihan Dadi
12. Kriti Patnala
13. Arnaav Goyal
14. Zack Martin
15. Samuel Babak
16. Nick Fogg
17. Jude Sproul
18. Casey Greene
19. Siddarth Shinde
