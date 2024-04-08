import axios from "axios";
import { SessionToken } from "./AuthApi";
import { getBackendServer, getAuthHeader } from "./Api";
import { User } from "./UserApi";

export class CreateRepositoryForm {
    name: string = "";
    isPublic: boolean = true;
}

export interface Repository {
    id: number;
    name: string;
    owner: User;
    ownerId: number;
    isPublic: boolean;
}

export interface Branch {
    name: string;
    hash: string;
}

export interface DirectoryEntry {
    name: string;
    isDirectory: boolean;
}

export const createRepository = async (token: SessionToken, form: CreateRepositoryForm): Promise<Repository> => {
    let resp = await axios.post(`http://${getBackendServer()}/repository`, form, {
        headers: getAuthHeader(token)
    });
    return resp.data as Repository;
}

export const getBranches = async (token: SessionToken | null, username: string, repoName: string): Promise<Branch[]> => {
    let headers = {}
    if (token != null) {
        headers = getAuthHeader(token);
    }

    let resp = await axios.get(`http://${getBackendServer()}/repository/${username}/${repoName}/branches`, {
        headers: headers
    });
    return resp.data as Branch[];
}

export const getRepos = async (token: SessionToken | null, username: string): Promise<Repository[]> => {
    let headers = {}
    if (token != null) {
        headers = getAuthHeader(token);
    }

    let resp = await axios.get(`http://${getBackendServer()}/repository/${username}`, {
        headers: headers
    });
    return resp.data as Repository[];
}

export const getListing = async (token: SessionToken | null, username: string, repoName: string, branch: string, path: string): Promise<DirectoryEntry[]> => {
    let headers = {}
    if (token != null) {
        headers = getAuthHeader(token);
    }

    let url = `http://${getBackendServer()}/repository/${username}/${repoName}/listing?branch=${branch}`;
    if (path != null && path != "") {
        url += `path=${path}`;
    }
    let resp = await axios.get(url, {
        headers: headers
    });
    return resp.data as DirectoryEntry[];
}
