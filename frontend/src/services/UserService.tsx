import ServiceProvider from "./ServiceProvider";
import ProvidedServices from "./ProvidedServices";
import {ITokenService, useTokenService} from "./TokenService";
import {useState} from "react";
import axios from "axios";
import {useAuthService} from "./AuthService";

export interface IUserService {
    retrieveCurrentUser(): void;
}

const UserServiceContext = ServiceProvider.createContext(ProvidedServices.UserService);
export const useUserService = () => ServiceProvider.use<IUserService>(ProvidedServices.UserService);

export function UserService({children}: any) {
    const authService = useAuthService();

    const [currentUser, setCurrentUser] = useState(null);

    const retrieveCurrentUser = () => {
        return axios
            .get("http://localhost:5000/users/me", {
                headers: authService.getAuthHeader()
            }).then(res => {
                setCurrentUser(res.data);
                return res.data;
            }).catch(error => {
                console.log(error);
                setCurrentUser(null);
                return error;
            })
    }

    const userService = {
        currentUser,
        retrieveCurrentUser
    }

    return (
        <UserServiceContext.Provider value={userService}>
            {children}
        </UserServiceContext.Provider>
    );
}
