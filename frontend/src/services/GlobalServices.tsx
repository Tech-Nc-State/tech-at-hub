import {TestService} from "./TestService";

/**
 * Services used application wide
 * @param children children of this component
 */
export function GlobalServices({children}: any) {
    return (
        <TestService>
            {children}
        </TestService>
    );
}
