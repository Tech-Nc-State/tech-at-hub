import type { NextPage } from 'next'
import {useUserService} from '../src/services/UserService'

const Home: NextPage = () => {

    const userService = useUserService();

    return (
        <div>
            {userService.currentUser != null ? userService.currentUser.name + "is logged in." : "Not logged in"}
        </div>
    );
}

export default Home;
