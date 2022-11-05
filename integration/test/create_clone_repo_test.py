import json
import subprocess
from util.api import ApiClient
from util.git import GitClient


def test_create_clone_repo(tmp_path):
    api = ApiClient()
    git = GitClient(str(tmp_path))

    # hit the signup endpoint
    r = api.post(
        "/user",
        {
            "firstName": "Joey",
            "lastName": "Bream",
            "username": "jbream",
            "password": "abcd1234",
            "email": "myemail@email.com",
            "birthDate": "1/1/2000",
        },
    )
    assert r.status_code == 200

    # hit the login endpoint
    r = api.post("/auth/login", {"username": "jbream", "password": "abcd1234"})
    assert r.status_code == 200
    r = json.loads(r.content)
    api.set_jwt(r["token"])

    # create a repository
    r = api.post("/repository", {"name": "test", "isPublic": True})
    assert r.status_code == 200

    # clone the repository
    git.run("git clone http://jbream:abcd1234@localhost/git/jbream/test.git")

    # cd into the repository
    git = GitClient(str(tmp_path) + "/test")

    # create a branch
    git.run("git checkout -b master")

    # create empty file, commit and push
    git.create("myfile.txt")
    git.run("git add .")
    git.run('git commit -m "Creating-new-file"')
    git.run("git push origin master")

    # query the api for the branch
    r = api.get("/repository/jbream/test/branches")
    assert r.status_code == 200

    b = json.loads(r.content)
    assert len(b) == 1
    assert b[0]["name"] == "master"

    git.run("git pull")
    git.run("git fetch")
