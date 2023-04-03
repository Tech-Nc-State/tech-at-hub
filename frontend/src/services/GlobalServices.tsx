import {UserService} from "./UserService";

/**
 * Services used application wide
 * @param children children of this component
 */
export function GlobalServices({children}: any) {
    return (
        <UserService>
            {children}
        </UserService>
    );
}
