import ServiceProvider from "./ServiceProvider";
import ProvidedServices from "./ProvidedServices";
import {ITokenService, useTokenService} from "./TokenService";
import {useState} from "react";
import axios from "axios";

export interface IUserService {

}

const UserServiceContext = ServiceProvider.createContext(ProvidedServices.UserService);
export const useUserService = () => ServiceProvider.use<IUserService>(ProvidedServices.UserService);

export function UserService({children}: any) {
    const tokenService = useTokenService();

    const [currentUser, setCurrentUser] = useState(null);

    const retrieveCurrentUser = () => {
        return axios
            .get("http://localhost:5000/users/me", {
                headers: {
                    Authorization: "Bearer " + tokenService.getTokenString()
                }
            }).then(res => {
                setCurrentUser(res.data);
            }).catch(error => {
                console.log(error);
                setCurrentUser(null);
            })

    }

    const userService = {

    }

    return (
        <UserServiceContext.Provider value={userService}>
            {children}
        </UserServiceContext.Provider>
    );
}
