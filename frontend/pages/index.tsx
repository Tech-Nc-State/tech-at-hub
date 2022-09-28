import type { NextPage } from 'next'
import {useTestService} from "../services/TestService";

const Home: NextPage = () => {

  const testService = useTestService();

  return(
      <div>{testService.getTestPhrase()}</div>
  );
}

export default Home
