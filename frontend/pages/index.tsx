import type { NextPage } from 'next'
import {useTestService} from "../src/services/TestService";

const Home: NextPage = () => {

  const testService = useTestService();

  return(
      <>
          <div>Price of Bitcoin: {testService.price}</div>
          <button onClick={testService.testMethod}>Show Test Method</button>
      </>
  );
}

export default Home
