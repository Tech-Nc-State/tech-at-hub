import axios, { AxiosResponse } from "axios";

export interface SessionToken {
    token: string;
    expiration: Date;
}

export const getAuthHeader = (token: SessionToken) => {
    return { Authorization: 'Bearer ' + token.token };
}

export const login = async (username: string, password: string): Promise<AxiosResponse> => {
    return axios.post("http://localhost:5000/auth/login", {
        username: username,
        password: password
    });
}

export const getMe = async (token: SessionToken): Promise<AxiosResponse> => {
    return axios.get("http://localhost:5000/user/me", {
        headers: getAuthHeader(token)
    });
};
