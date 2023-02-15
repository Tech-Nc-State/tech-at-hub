import ServiceProvider from "./ServiceProvider";
import ProvidedServices from "./ProvidedServices";
import {useState} from "react";
import axios from "axios";

export interface IUserService {
    setCurrentUser(user: any): void;
    getUser(): any;
    voidUser(): void;
    hasUser(): boolean;
};

const UserServiceContext = ServiceProvider.createContext<IUserService>(ProvidedServices.UserService);
export const useUserService = () => ServiceProvider.use<IUserService>(ProvidedServices.UserService);

export function UserService({children}: any) {
    const [user, setUser] = useState(null);

    const setCurrentUser = (user: any) => {
        setUser(user);
    }

    const getUser = () => {
        return user;
    }

    const voidUser = () => {
        setUser(null);
    }

    const hasUser = () => {
        return user != null ? true : false;
    }

    const userService = {
        setCurrentUser,
        getUser,
        voidUser,
        hasUser,
    }

    return (
        <UserServiceContext.Provider value={userService}>
            {children}
            </UserServiceContext.Provider>
    );
}
