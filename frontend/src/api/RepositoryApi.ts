import axios, { AxiosResponse } from "axios";
import { SessionToken } from "./AuthApi";
import { getBackendServer, getAuthHeader } from "./Api";

export class CreateRepositoryForm {
    name: string = "";
    isPublic: boolean = true;
  }

export const createRepository = (token: SessionToken, form: CreateRepositoryForm): Promise<AxiosResponse> => {
    return axios.post(`http://${getBackendServer()}/repository`, form, {
        headers: getAuthHeader(token)
    });
}

export const getBranches = (token: SessionToken | null, username: string, repoName: string): Promise<AxiosResponse> => {
    let headers = {}
    if (token != null) {
        headers = getAuthHeader(token);
    }

    return axios.get(`http://${getBackendServer()}/repository/${username}/${repoName}/branches`, {
        headers: headers
    });
}
