import { Box, Button, Typography } from "@mui/material";
import { getMe } from "../api/UserApi";
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
  const [user, setUser] = useState<any>(null);

  useEffect(() => {
    if (sessionService.hasSessionToken()) {
      getMe(sessionService.getSessionToken()).then((response) => {
        setUser(response.data);
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
      {sessionService.hasSessionToken() && (
        <Typography>Hello {user?.username}</Typography>
      )}
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
          {sessionService.hasSessionToken() && (
            <Button variant="contained" onClick={logout} sx={{ m: "10px" }}>
              Logout
              <FontAwesomeIcon
                icon={faRightFromBracket}
                style={{ marginLeft: "10px" }}
              />
            </Button>
          )}
          {sessionService.hasSessionToken() && (
            <Button variant="contained" href="/new" sx={{ m: "10px" }}>
              Create Repository
            </Button>
          )}
        </Box>
      </Box>
    </Box>
  );
}

export default HomePage;
