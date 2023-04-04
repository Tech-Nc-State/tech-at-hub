import axios, { AxiosResponse } from "axios";

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
    return axios.post("http://localhost:5000/user", form);
}

export const getProfilePicture = (username: string): Promise<AxiosResponse> => {
    return axios.get(`http://localhost:5000/user/${username}/profilepicture`, {
        headers: {
            'Content-Type': 'image/jpeg'
        },
        responseType: "arraybuffer"
    });
}
