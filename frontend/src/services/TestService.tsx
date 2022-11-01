import ServiceProvider from "./ServiceProvider";
import ProvidedServices from "./ProvidedServices";
import axios from "axios";
import {useEffect, useRef, useState} from "react";

export interface ITestService {
    price: string,
    testMethod(): void
}

const TestServiceContext = ServiceProvider.createContext(ProvidedServices.TestService);
export const useTestService = () => ServiceProvider.use<ITestService>(ProvidedServices.TestService)

/**
 * Simple test service to demonstrate the capabilities of the Context API for services in the application
 */
export function TestService({children}: any) {

    const [price, setPrice] = useState("");

    const getBitcoinPrice = () => {
        axios.get("https://api.coindesk.com/v1/bpi/currentprice.json")
            .then(response => {
                let updatedPrice = response.data.bpi.USD.rate;
                setPrice(updatedPrice)
            })
    }

    const countInterval = useRef<any>(null);
    useEffect(() => {
        countInterval.current = setInterval(
            () => {
                getBitcoinPrice();
            }, 10000);

        return () => {
            clearInterval(countInterval.current);
        }
    }, [price]);

    const testService = {
        price,
        testMethod() {
            alert("Can also pass methods from the service");
        }
    }

    return (
        <TestServiceContext.Provider value={testService}>
            {children}
        </TestServiceContext.Provider>
    );
}
