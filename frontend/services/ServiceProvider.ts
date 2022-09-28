// ServiceProvider class largely derived from a Contextualizer class found on
// https://dev.to/dansolhan/simple-dependency-injection-functionality-for-react-518j

import React from "react";
import ProvidedServices from "./ProvidedServices";

const contexts = new Map<ProvidedServices, React.Context<any | undefined>>();

const ServiceProvider = {
    createContext: <T>(service: ProvidedServices): React.Context<T | undefined> => {
        const context = React.createContext<T | undefined>(undefined);
        contexts.set(service, context);
        return context;
    },

    use: <T>(services: ProvidedServices): T => {
        const context = contexts.get(services);
        if (context === undefined) {
            throw new Error(`${ProvidedServices[services]} was not created`);
        }
        const service = React.useContext(context);

        if (service === undefined) {
            throw new Error(`You must use ${ProvidedServices[services]} from within its service`)
        }
        return service;
    },

    clear() {
        contexts.clear()
    }
}

export default ServiceProvider
