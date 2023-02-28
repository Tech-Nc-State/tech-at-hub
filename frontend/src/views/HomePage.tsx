import { Box, Button } from "@mui/material";
import { Link } from "react-router-dom";
import { useUserService } from "../services/UserService";

function HomePage() {

    const userService = useUserService();

    return (
        <Box>
            <p>Home Page</p>
            <p>
                {userService.getUser() != null ? userService.getUser.name + "is logged in." : "Not logged in"}
            </p>
            <Button variant="contained" href="/login">
                Login
            </Button>
            <Button variant="contained" href="/signup">
                Signup
            </Button>
        </Box>
    );
}

export default HomePage;
