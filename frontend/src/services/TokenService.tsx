import ServiceProvider from "./ServiceProvider";
import ProvidedServices from "./ProvidedServices";
import {useEffect, useRef, useState} from "react";

export interface ITokenService {
    token: {token: string, expiration: string},
    addToken(): void
}

const TokenServiceContext = ServiceProvider.createContext(ProvidedServices.TokenService);
export const useTokenService = () => ServiceProvider.use<ITokenService>(ProvidedServices.TokenService);

const TOKEN_KEY = "token";

export function TokenService({children}: any) {

    const getToken = () => {
        let tokenResult = localStorage.getItem(TOKEN_KEY);
        if (!tokenResult) return null;
        try {
            return JSON.parse(tokenResult);
        } catch (error) {
            console.log(error);
            return null;
        }
    }

    const setToken = (token: {token: string, expiration: string}) => {
        localStorage.setItem(TOKEN_KEY, JSON.stringify(token));
    }

    const invalidateToken = () => {
        localStorage.removeItem(TOKEN_KEY)
    }

    const tokenService = {
        getToken,
        setToken,
        invalidateToken,
    }

    return (
        <TokenServiceContext.Provider value={tokenService}>
            {children}
        </TokenServiceContext.Provider>
    )


}
