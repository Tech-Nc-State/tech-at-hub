import { SessionToken } from "./AuthApi";

export const getBackendServer = (): string => {
    // return 'localhost:80/api';
    return 'localhost:5000';
}

export const getAuthHeader = (token: SessionToken) => {
    return { Authorization: 'Bearer ' + token.token };
}
