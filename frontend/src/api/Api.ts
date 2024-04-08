import { SessionToken } from "./AuthApi";

export const getBackendServer = (): string => {
    // return 'localhost:80/api';  // in docker
    return 'localhost:5000'; // in local
}

export const getAuthHeader = (token: SessionToken) => {
    return { Authorization: 'Bearer ' + token.token };
}
