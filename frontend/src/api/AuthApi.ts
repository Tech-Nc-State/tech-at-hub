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

export const login = async (credentials: Credentials): Promise<SessionToken> => {
    let resp = await axios.post(`http://${getBackendServer()}/auth/login`, credentials);
    return resp.data as SessionToken;
}
