import { Box, Button, Typography } from "@mui/material";
import { User, getMe } from "../api/UserApi";
import { useSessionService } from "../services/SessionService";
import { useState, useEffect } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faRightFromBracket,
  faRightToBracket,
  faUserPlus,
} from "@fortawesome/free-solid-svg-icons";

function HomePage() {
  const sessionService = useSessionService();
  const [user, setUser] = useState<User | null>(null);

  useEffect(() => {
    if (sessionService.hasSessionToken()) {
      getMe(sessionService.getSessionToken()).then((user) => {
        setUser(user);
      });
    }
  }, []);

  const logout = () => {
    sessionService.clearSession();
    setUser(null);
    window.location.reload();
  };

  return (
    <Box>
      <Typography sx={{ fontSize: "3rem", fontWeight: "700" }}>Home</Typography>
      <hr></hr>

      {sessionService.hasSessionToken() && (
        <Typography sx={{ fontSize: "1.2rem"}}>Hello {user?.username}!</Typography>
      )}
      {/* Contextual button depending on whether logged in or not */}

      {sessionService.hasSessionToken() ?
        // Show This if we have Token
        // This should be anything we want a logged in user to see
        // TODO: This could also be just a single component if we want to refactor.
        <Box sx={{ display: "flex", justifyContent: "center" }}>
          <Box
            sx={{
              display: "flex",
              flexDirection: "column",
              width: "50%",
              alignItems: "center",
            }}
          >
            <Button variant="contained" onClick={logout} sx={{ m: "10px" }}>
              Logout
              <FontAwesomeIcon
                icon={faRightFromBracket}
                style={{ marginLeft: "10px" }}
              />
            </Button>
            <Button variant="contained" href="/new" sx={{ m: "10px" }}>
              Create Repository
            </Button>
          </Box>
        </Box>

        : // ELSE

        // This is the stuff we show when not logged in
        // Right now, it's just the login and signup button
        <Box sx={{ display: "flex", justifyContent: "center" }}>
          <Box
            sx={{
              display: "flex",
              flexDirection: "column",
              width: "50%",
              alignItems: "center",
            }}
          >
            <Button variant="contained" href="/login" sx={{ m: "10px" }}>
              Login
              <FontAwesomeIcon
                icon={faRightToBracket}
                style={{ marginLeft: "10px" }}
              />
            </Button>
            <Button variant="contained" href="/signup" sx={{ m: "10px" }}>
              Signup
              <FontAwesomeIcon icon={faUserPlus} style={{ marginLeft: "10px" }} />
            </Button>
          </Box>
        </Box>
      }

    </Box>
  );
}

export default HomePage;
