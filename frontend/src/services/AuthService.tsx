import ServiceProvider from "./ServiceProvider";
import ProvidedServices from "./ProvidedServices";
import axios from "axios";
import {useTokenService} from "./TokenService";

export interface IAuthService {
  login(username: string, password: string): void;
  logout(): void;
  getAuthHeader(): { Authorization: string };
}

const AuthServiceContext = ServiceProvider.createContext(
  ProvidedServices.AuthService
);
export const useAuthService = () =>
  ServiceProvider.use<IAuthService>(ProvidedServices.AuthService);

export function AuthService({ children }: any) {

  const tokenService = useTokenService();

  const login = (username: string, password: string) => {
    axios
      .post("http://localhost:5000/auth/login", {
        username: username,
        password: password,
      })
      .then((response) => {
        console.log(response);
        tokenService.setToken(response.data.token)
        return response;
      })
      .catch((error) => {
        console.log(error);
        return error;
      });
  };

  const logout = () => {
    tokenService.invalidateToken()
  }

  const getAuthHeader = () => {
    return { Authorization: 'Bearer ' + tokenService.getTokenString() }
  }

  const authService = {
    login,
    logout,
    getAuthHeader,
  };
  return (
    <AuthServiceContext.Provider value={authService}>
      {children}
    </AuthServiceContext.Provider>
  );
}
