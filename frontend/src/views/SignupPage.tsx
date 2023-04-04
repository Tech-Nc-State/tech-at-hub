import { Box, Button, TextField, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { useSessionService } from "../services/SessionService";
import { SessionToken, login } from "../api/AuthApi";
import { useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faLock,
  faUser,
  faSignature,
  faEnvelope,
  faCakeCandles,
} from "@fortawesome/free-solid-svg-icons";
import { SignupForm, signup } from "../api/UserApi";
import { AxiosResponse } from "axios";

function SignupPage() {
  const sessionService = useSessionService();
  const navigate = useNavigate();
  const [signupInfo, setSignupInfo] = useState(new SignupForm());
  const [validation, setValidation] = useState("");

  useEffect(() => {
    if (sessionService.hasSessionToken()) {
      navigate("/");
    }
  }, []);

  const signupHandler = async () => {
    // check the passwords match
    if (signupInfo.password != signupInfo.confirmPassword) {
      setValidation("Confirm Password must match Password");
      return;
    }

    try {
      // hit the signup API
      await signup(signupInfo);

      // login the new account
      let sessionTokenResponse = await login(
        signupInfo.username,
        signupInfo.password
      );
      let sessionToken: SessionToken = sessionTokenResponse.data;
      // save the token to a session
      sessionService.createSession(sessionToken);
      // redirect to home page
      navigate("/");
    } catch (err: any) {
      setValidation(err.response.data);
    }
  };

  return (
    <Box>
      <Typography sx={{ fontSize: "3rem", fontWeight: "700" }}>
        Sign Up
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
            <FontAwesomeIcon icon={faSignature} />
            <TextField
              id="standard-basic"
              label="First Name"
              value={signupInfo.firstName}
              onChange={(event) => {
                setSignupInfo({
                  ...signupInfo,
                  firstName: event.target.value,
                });
              }}
              sx={{ m: "10px" }}
            />
          </Box>

          <Box sx={{ display: "flex", alignItems: "center" }}>
            <FontAwesomeIcon icon={faSignature} />
            <TextField
              id="standard-basic"
              label="Last Name"
              value={signupInfo.lastName}
              onChange={(event) => {
                setSignupInfo({
                  ...signupInfo,
                  lastName: event.target.value,
                });
              }}
              sx={{ m: "10px" }}
            />
          </Box>

          <Box sx={{ display: "flex", alignItems: "center" }}>
            <FontAwesomeIcon icon={faUser} />
            <TextField
              id="standard-basic"
              label="Username"
              value={signupInfo.username}
              onChange={(event) => {
                setSignupInfo({
                  ...signupInfo,
                  username: event.target.value,
                });
              }}
              sx={{ m: "10px" }}
            />
          </Box>

          <Box sx={{ display: "flex", alignItems: "center" }}>
            <FontAwesomeIcon icon={faLock} />
            <TextField
              id="standard-basic"
              label="Password"
              type="password"
              value={signupInfo.password}
              onChange={(event) => {
                setSignupInfo({
                  ...signupInfo,
                  password: event.target.value,
                });
              }}
              sx={{ m: "10px" }}
            />
          </Box>

          <Box sx={{ display: "flex", alignItems: "center" }}>
            <FontAwesomeIcon icon={faLock} />
            <TextField
              id="standard-basic"
              label="Confirm Password"
              type="password"
              value={signupInfo.confirmPassword}
              onChange={(event) => {
                setSignupInfo({
                  ...signupInfo,
                  confirmPassword: event.target.value,
                });
              }}
              sx={{ m: "10px" }}
            />
          </Box>

          <Box sx={{ display: "flex", alignItems: "center" }}>
            <FontAwesomeIcon icon={faEnvelope} />
            <TextField
              id="standard-basic"
              label="Email Address"
              type="email"
              value={signupInfo.email}
              onChange={(event) => {
                setSignupInfo({
                  ...signupInfo,
                  email: event.target.value,
                });
              }}
              sx={{ m: "10px" }}
            />
          </Box>

          <Box sx={{ display: "flex", alignItems: "center" }}>
            <FontAwesomeIcon icon={faCakeCandles} />
            <TextField
              id="standard-basic"
              label="Birth Date (MM/dd/yyyy)"
              value={signupInfo.birthdate}
              onChange={(event) => {
                setSignupInfo({
                  ...signupInfo,
                  birthdate: event.target.value,
                });
              }}
              sx={{ m: "10px" }}
            />
          </Box>

          <Button variant="contained" onClick={signupHandler}>
            Sign Up
          </Button>
        </Box>
      </Box>
    </Box>
  );
}

export default SignupPage;
