import {TokenService} from "./TokenService";
import {UserService} from "./UserService";
import {AuthService} from "./AuthService";

/**
 * Services used application wide
 * @param children children of this component
 */
export function GlobalServices({children}: any) {
    return (
        <TokenService>
            <AuthService>
                <UserService>
                    {children}
                </UserService>
            </AuthService>
        </TokenService>
    );
}
