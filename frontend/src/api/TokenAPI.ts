const TOKEN_KEY = "token";

export function getToken() {
    let tokenResult = localStorage.getItem(TOKEN_KEY);
    if (!tokenResult) return null;
    try {
        return JSON.parse(tokenResult);
    } catch (error) {
        console.log(error);
        return null;
    }
}

export function getTokenString() {
    let tokenString = localStorage.getItem(TOKEN_KEY);
    if (!tokenString) return null;
    return tokenString;
}

export function setToken(token: {token: string, expiration: string}) {
    localStorage.setItem(TOKEN_KEY, JSON.stringify(token));
}

export function invalidateToken() {
    localStorage.removeItem(TOKEN_KEY)
}

export function getAuthHeader() {
    return { Authorization: 'Bearer ' + getTokenString() };
}
