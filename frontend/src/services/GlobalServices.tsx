import {SessionService} from "./SessionService";

/**
 * Services used application wide
 * @param children children of this component
 */
export function GlobalServices({children}: any) {
    return (
        <SessionService>
            {children}
        </SessionService>
    );
}
