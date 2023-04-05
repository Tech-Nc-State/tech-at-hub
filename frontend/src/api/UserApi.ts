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

export const signup = (form: SignupForm): Promise<AxiosResponse> => {
    return axios.post(`http://${getBackendServer()}/user`, form);
}

export const getProfilePicture = (username: string): Promise<AxiosResponse> => {
    return axios.get(`http://${getBackendServer()}/user/${username}/profilepicture`, {
        headers: {
            'Content-Type': 'image/jpeg'
        },
        responseType: "arraybuffer"
    });
}

export const getMe = async (token: SessionToken): Promise<AxiosResponse> => {
    return axios.get(`http://${getBackendServer()}/user/me`, {
        headers: getAuthHeader(token)
    });
};
