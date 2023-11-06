# tech-at-hub

Code-hosting website built for fun by students at NC State University

## Environment Setup

We use pre-commit to automatically manage our git hooks. pre-commit is required to develop in this project. We enforce a strict LF-only line ending policy. Our Dockerfiles will break if any CRLF's are introduced into the project. One of the pre-commit hooks is a LF enforcer hook. To setup:

1. Run `pip install pre-commit`
2. Run `pre-commit install`

In addition, to prevent git from auto-converting LF's to CRLF's on Windows machines, run

```bash
git config core.autocrlf false
```

## Project Configuration

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
