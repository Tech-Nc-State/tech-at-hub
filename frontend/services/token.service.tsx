class TokenService {

    static tokenKey: string = "token";

    /**
     *
     * @param tokenData { token: string, expiration: date }
     */
    static setToken(tokenData: { token: string, expiration: string }) {
        if (!this.isTokenExpired(tokenData.expiration)) {
            localStorage.setItem(TokenService.tokenKey, JSON.stringify(tokenData))
            return Promise.resolve("Set the token");
        } else {
            return Promise.reject("Unable to set the token because it was expired");
        }
    }

    static retrieveToken() {
        let result = window.localStorage.getItem(TokenService.tokenKey);
        if (!result) {
            return null;
        }
        return JSON.parse(result).token;
    }

    static removeToken() {
        localStorage.removeItem(TokenService.tokenKey);
    }

    private static isTokenExpired(expiration: string) {
        let expirationDate = Date.parse(expiration)
        return expirationDate < Date.now();
    }

    static getAuthHeader(tokenData: { token: string, expiration: string }) {
        let token = localStorage.getItem(TokenService.tokenKey)
        if (!this.isTokenExpired(tokenData.expiration)) {
            if (token) {
                let tokenJSON = JSON.parse(token)
                if (tokenJSON) {
                    return { Authorization: 'Bearer ' + tokenJSON };
                }
            }
        }
        return null;
    }
}
export default TokenService;
