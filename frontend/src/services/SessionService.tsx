import ServiceProvider from "./ServiceProvider";
import ProvidedServices from "./ProvidedServices";
import { SessionToken } from "../api/AuthApi";

export interface ISessionService {
    createSession(token: any): void;
    clearSession(): void;
    getSessionToken(): any;
    hasSessionToken(): boolean;
};

const SessionServiceContext = ServiceProvider.createContext<ISessionService>(ProvidedServices.SessionService);
export const useSessionService = () => ServiceProvider.use<ISessionService>(ProvidedServices.SessionService);

export function SessionService({children}: any) {
    const TOKEN_KEY = "token";

    const createSession = (token: SessionToken) => {
        localStorage.setItem(TOKEN_KEY, JSON.stringify(token));
    }

    const clearSession = () => {
        localStorage.removeItem(TOKEN_KEY);
    }

    const getSessionToken = (): SessionToken | null => {
        let tokenString = localStorage.getItem(TOKEN_KEY);
        if (!tokenString) return null;
        try {
            return JSON.parse(tokenString);
        }
        catch {
            return null;
        }
    }

    const hasSessionToken = (): boolean => {
        let tokenString = localStorage.getItem(TOKEN_KEY);
        return tokenString != null && tokenString != undefined;
    }

    const sessionService = {
        createSession,
        clearSession,
        getSessionToken,
        hasSessionToken
    }

    return (
        <SessionServiceContext.Provider value={sessionService}>
            {children}
        </SessionServiceContext.Provider>
    );
}
