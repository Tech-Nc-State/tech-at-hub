import Axios from 'axios'
import TokenService from "./token.service";
const API_URL = "http://localhost:5000/"

class AuthService {



    //TODO add birthdate here after figuring out what object is returned by the form
    static signUp(signUpData: {firstName: string, lastName: string,
           username: string, password: string, email: string}) {
        return Axios.post(API_URL + 'user', {
            firstName: signUpData.firstName,
            lastName: signUpData.lastName,
            username: signUpData.username,
            password: signUpData.password,
            email: signUpData.email,
            //TODO fix hardcoded date for new users
            birthDate: Date.now().toLocaleString()
        }).then(response => {
            return {response: response, username: signUpData.username, password: signUpData.password}
        })
    }

    static login(username: string, password: string) {
        return Axios.post(API_URL + 'auth/login', {
            username: username,
            password: password
        }).then(response => {
            if (response.data.token) {
                TokenService.setToken(response.data).catch(err => {
                    console.log("Token was expired")
                });
            }
            return response.data;
        })
    }

    static logout() {
        TokenService.removeToken();
    }
}

export default AuthService;
