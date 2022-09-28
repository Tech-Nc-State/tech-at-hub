import ServiceProvider from "./ServiceProvider";
import ProvidedServices from "./ProvidedServices";
import {useContext} from "react";

export interface ITestService {
    getTestPhrase(): string
}

const TestServiceContext = ServiceProvider.createContext(ProvidedServices.TestService);
export const useTestService = () => ServiceProvider.use<ITestService>(ProvidedServices.TestService)

export function TestService({children}: any) {
    let testPhrase: string = "testString";

    const testService = {
        getTestPhrase(): string {
            return testPhrase;
        }
    }

    return (
        <TestServiceContext.Provider value={testService}>
            {children}
        </TestServiceContext.Provider>
    );
}
