import { Box, Button, TextField, Typography, Avatar, Grid, ListItem } from "@mui/material";
import { useEffect, useState } from "react";
import { useSessionService } from "../services/SessionService";
import { Credentials, SessionToken, login } from "../api/AuthApi";
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

function ProfilePage() {
//   const sessionService = useSessionService();
//   const navigate = useNavigate();
//   const [signupInfo, setSignupInfo] = useState(new SignupForm());
//   const [validation, setValidation] = useState("");

//   useEffect(() => {
//     if (sessionService.hasSessionToken()) {
//       navigate("/");
//     }
//   }, []);

//   const signupHandler = async () => {
//     // check the passwords match
//     if (signupInfo.password != signupInfo.confirmPassword) {
//       setValidation("Confirm Password must match Password");
//       return;
//     }

//     try {
//       // hit the signup API
//       await signup(signupInfo);

//       // login the new account
//       let sessionToken: SessionToken = await login({
//         username: signupInfo.username,
//         password: signupInfo.password,
//       } as Credentials);
//       // save the token to a session
//       sessionService.createSession(sessionToken);
//       // redirect to home page
//       navigate("/");
//     } catch (err: any) {
//       setValidation(err.response.data);
//     }
//   };

  return (
    
    <Box>
      <Grid container spacing={2}>
        <Grid item xs={6}>
            <Avatar
                alt="Remy Sharp"
                src="/static/images/avatar/1.jpg"
                sx={{ width: 256, height: 256 }}
            />
        </Grid>
        <Grid item xs={3}>
            <Box>
                <Typography sx={{ fontSize: "3rem", fontWeight: "700" }}>
                    @username
                </Typography>
                <Typography sx={{ fontSize: "2rem", fontWeight: "700" }}>
                    First Last
                </Typography>
            </Box>
        </Grid>
        <Grid item xs={4}>
        </Grid>
        <Grid item xs={8}>
        </Grid>
      </Grid>

      <Box sx={{ display: "flex", justifyContent: "center" }}>
        <Box
          sx={{
            display: "flex",
            flexDirection: "column",
            width: "50%",
            alignItems: "center",
          }}
        >
        </Box>
      </Box>
    </Box>
  );
}

export default ProfilePage;
