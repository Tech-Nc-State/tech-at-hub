import {TestService} from "./TestService";

export function GlobalServices({children}: any) {
    return (
        <TestService>
            {children}
        </TestService>
    );
}
