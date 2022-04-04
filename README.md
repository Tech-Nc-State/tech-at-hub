# tech-at-hub

Code-hosting website built for fun by students at NC State University

## Project Configuration

```
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
