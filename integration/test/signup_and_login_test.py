import json
from util.api import ApiClient


def test_signup_and_login():
    api = ApiClient()

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

    # hit the 'get me' endpoint
    r = api.get(f"/user/me")
    assert r.status_code == 200
    r = json.loads(r.content)
    assert r["username"] == "jbream"

    # change password
    r = api.post(
        "/user/password",
        {
            "username": "jbream",
            "oldPassword": "abcd1234",
            "newPassword": "1234abcd",
            "newPasswordRetyped": "1234abcd",
        },
    )
    assert r.status_code == 200
