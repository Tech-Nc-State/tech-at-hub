import ServiceProvider from "./ServiceProvider";
import ProvidedServices from "./ProvidedServices";
import axios from "axios";
import { useEffect, useRef, useState } from "react";

export interface IApiService {
  login(user: string, pass: string): void;
}

const ApiServiceContext = ServiceProvider.createContext(
  ProvidedServices.ApiService
);
export const useApiService = () =>
  ServiceProvider.use<IApiService>(ProvidedServices.ApiService);

export function ApiService({ children }: any) {
  const [token, setToken] = useState("");

  const loginUser = (user: string, pass: string) => {
    axios
      .post("http://localhost:5000/auth/login", {
        username: user,
        password: pass,
      })
      .then((response) => {
        setToken(response.data.token);
        alert(`You are logged in. ${response.data.token}`);
      })
      .catch((error) => {
        alert(error);
      });
  };

  const apiService = {
    login(user: string, pass: string) {
      loginUser(user, pass);
    },
  };
  return (
    <ApiServiceContext.Provider value={apiService}>
      {children}
    </ApiServiceContext.Provider>
  );
}
