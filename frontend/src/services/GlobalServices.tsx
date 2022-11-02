import {TestService} from "./TestService";
import {TokenService} from "./TokenService";

/**
 * Services used application wide
 * @param children children of this component
 */
export function GlobalServices({children}: any) {
    return (
        <TokenService>
            {children}
        </TokenService>
    );
}
