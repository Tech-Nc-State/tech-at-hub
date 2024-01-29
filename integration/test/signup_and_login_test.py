import json
from util.api import ApiClient


def test_signup_and_login():
    api = ApiClient()

    user = "jbream"
    pw = "Abcd1234_"

    # hit the signup endpoint
    r = api.post(
        "/user",
        {
            "firstName": "Joey",
            "lastName": "Bream",
            "username": user,
            "password": pw,
            "email": "myemail@email.com",
            "birthDate": "1/1/2000",
        },
    )
    assert r.status_code == 200

    # hit the login endpoint
    r = api.post("/auth/login", {"username": user, "password": pw})
    assert r.status_code == 200
    r = json.loads(r.content)
    api.set_jwt(r["token"])

    # hit the 'get me' endpoint
    r = api.get(f"/user/me")
    assert r.status_code == 200
    r = json.loads(r.content)
    assert r["username"] == user

    # change password
    newpw = "1234Abcd_"
    r = api.post(
        "/user/password",
        {
            "username": user,
            "oldPassword": pw,
            "newPassword": newpw,
            "newPasswordRetyped": newpw,
        },
    )
    assert r.status_code == 200
