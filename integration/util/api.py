import requests


class ApiClient:
    def __init__(self):
        self.jwt = ""
        self.prefix = "http://localhost:80/api"

    def get(self, url, send_jwt=True) -> requests.Response:
        headers = {}
        if send_jwt and self.jwt != "":
            headers["Authorization"] = f"Bearer {self.jwt}"
        r = requests.get(f"{self.prefix}{url}", headers=headers)
        return r

    def post(self, url, body_dict, send_jwt=True) -> requests.Response:
        headers = {}
        if send_jwt and self.jwt != "":
            headers["Authorization"] = f"Bearer {self.jwt}"
        r = requests.post(f"{self.prefix}{url}", json=body_dict, headers=headers)
        return r

    def put(self, url, body_dict, send_jwt=True) -> requests.Response:
        headers = {}
        if send_jwt and self.jwt != "":
            headers["Authorization"] = f"Bearer {self.jwt}"
        r = requests.put(f"{self.prefix}{url}", json=body_dict, headers=headers)
        return r

    def set_jwt(self, jwt):
        self.jwt = jwt
