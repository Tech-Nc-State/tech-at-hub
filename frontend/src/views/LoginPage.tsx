import { Box, Button, TextField, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { useSessionService } from "../services/SessionService";
import { Credentials, SessionToken, login } from "../api/AuthApi";
import { useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faLock, faUser } from "@fortawesome/free-solid-svg-icons";

function LoginPage() {
  const sessionService = useSessionService();
  const navigate = useNavigate();
  const [loginInfo, setLoginInfo] = useState(new Credentials());
  const [errorState, setErrorState] = useState(false);

  useEffect(() => {
    if (sessionService.hasSessionToken()) {
      navigate("/");
    }
  }, []);

  const loginHandler = async () => {
    try {
      // use the API to get a token
      let sessionToken: SessionToken = await login(loginInfo);
      // save the token to a session
      sessionService.createSession(sessionToken);
      // redirect to home page
      navigate("/");
      window.location.reload();
    } catch (err) {
      console.log(err); // todo: stop leaking this
      setErrorState(true);
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

          {errorState ?
            <Box sx={{ color: "#FF0000"}}>Error: The username or password is incorrect.</Box>
            :
            ""
          }

        </Box>
      </Box>
    </Box>
  );
}

export default LoginPage;
