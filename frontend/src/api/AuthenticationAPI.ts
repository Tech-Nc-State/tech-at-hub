import { getTokenString, invalidateToken, setToken } from "./TokenAPI";
import axios from "axios";

// FILL THIS OUT
const URL = "";

export function login(username: string, password: string) {
    axios.post(URL + "/auth/login", {
        username: username,
        password: password
    }).then((response) => {
        console.log(response);
        setToken(response.data.token);
        return response;
    }).catch((error) => {
        console.log(error);
        return error;
    });
}

export function getAuthHeader() {
    return { Authorization: 'Bearer ' + getTokenString() };
}

export function logout() {
    invalidateToken();
}
