import axios, { AxiosResponse } from "axios";
import { SessionToken } from "./AuthApi";
import { getBackendServer, getAuthHeader } from "./Api";

export class SignupForm {
    firstName: string = "";
    lastName: string = "";
    username: string = "";
    password: string = "";
    confirmPassword: string = "";
    email: string = "";
    birthdate: string = "";
}

export interface User {
    id: number;
    username: string;
    email: string;
    firstName: string;
    lastName: string;
    description: string;
    birthDate: Date;
};

export const signup = async (form: SignupForm): Promise<User> => {
    let resp = await axios.post(`http://${getBackendServer()}/user`, form);
    return resp.data as User;
}

export const getProfilePicture = async (username: string): Promise<Uint8Array> => {
    let resp = await axios.get(`http://${getBackendServer()}/user/${username}/profilepicture`, {
        headers: {
            'Content-Type': 'image/jpeg'
        },
        responseType: "arraybuffer"
    });
    return new Uint8Array(resp.data);
}

export const getMe = async (token: SessionToken): Promise<User> => {
    let resp = await axios.get(`http://${getBackendServer()}/user/me`, {
        headers: getAuthHeader(token)
    });
    return resp.data as User;
};
