import {TokenService} from "./TokenService";
import {UserService} from "./UserService";

/**
 * Services used application wide
 * @param children children of this component
 */
export function GlobalServices({children}: any) {
    return (
        <TokenService>
            <UserService>
                {children}
            </UserService>
        </TokenService>
    );
}
