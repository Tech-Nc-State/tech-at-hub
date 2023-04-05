import axios, { AxiosResponse } from "axios";
import { getBackendServer } from "./Api";

export interface SessionToken {
    token: string;
    expiration: Date;
}

export class Credentials {
    username: string = "";
    password: string = "";
}

export const login = async (credentials: Credentials): Promise<AxiosResponse> => {
    return axios.post(`http://${getBackendServer()}/auth/login`, credentials);
}
