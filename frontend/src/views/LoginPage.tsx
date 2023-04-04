import { Box, Button, TextField, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { useSessionService } from "../services/SessionService";
import { SessionToken, login } from "../api/AuthApi";
import { useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faLock, faUser } from "@fortawesome/free-solid-svg-icons";

function LoginPage() {
  const sessionService = useSessionService();
  const navigate = useNavigate();
  const [loginInfo, setLoginInfo] = useState({ username: "", password: "" });

  useEffect(() => {
    if (sessionService.hasSessionToken()) {
      navigate("/");
    }
  }, []);

  const loginHandler = async () => {
    // use the API to get a token
    let sessionTokenResponse = await login(
      loginInfo.username,
      loginInfo.password
    );
    if (sessionTokenResponse.status == 200) {
      let sessionToken: SessionToken = sessionTokenResponse.data;
      // save the token to a session
      sessionService.createSession(sessionToken);
      // redirect to home page
      navigate("/");
    }
  };

  return (
    <Box>
      <Typography sx={{ fontSize: "3rem", fontWeight: "700" }}>
        Login
      </Typography>
      <Box sx={{ display: "flex", justifyContent: "center" }}>
        <Box
          sx={{
            display: "flex",
            flexDirection: "column",
            width: "50%",
            alignItems: "center",
          }}
        >
          <Box sx={{ display: "flex", alignItems: "center" }}>
            <FontAwesomeIcon icon={faUser} />
            <TextField
              id="standard-basic"
              label="Username"
              value={loginInfo.username}
              onChange={(event) => {
                setLoginInfo({
                  ...loginInfo,
                  username: event.target.value,
                });
              }}
              sx={{ m: "10px" }}
            />
          </Box>

          <Box sx={{ display: "flex", alignItems: "center" }}>
            <FontAwesomeIcon icon={faLock} style={{}} />
            <TextField
              id="standard-basic"
              label="Password"
              type="password"
              value={loginInfo.password}
              onChange={(event) => {
                setLoginInfo({
                  ...loginInfo,
                  password: event.target.value,
                });
              }}
              sx={{ m: "10px" }}
            />
          </Box>
          <Button variant="contained" onClick={loginHandler}>
            Login
          </Button>
        </Box>
      </Box>
    </Box>
  );
}

export default LoginPage;
