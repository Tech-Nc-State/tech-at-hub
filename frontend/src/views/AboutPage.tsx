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
      {/* Write HTML below!  */}
      <p>Hi THERE</p>
    </Box>
  );
}

export default HomePage;
