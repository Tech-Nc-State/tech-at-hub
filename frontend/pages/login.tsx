import Textfield from "@mui/material/TextField";
import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import Button from "@mui/material/Button";
import {useAuthService} from "../src/services/AuthService";

export default function LoginPage() {
  const authService = useAuthService();

  return (
    <div>
      <Box
        text-align="center"
        margin="auto"
        sx={{
          bgcolor: "lightgrey",
          width: "70%",
          align: "center",
          marginTop: "20px",
        }}
      >
        <Grid container direction="column" alignItems="center">
          <Textfield
            variant="outlined"
            label="Username"
            style={{ marginBottom: "2em" }}
          ></Textfield>
          <Textfield
            variant="outlined"
            label="Password"
            style={{ marginBottom: "2em" }}
          ></Textfield>
          <Button
            variant="contained"
            size="large"
            color="secondary"
            onClick={() => {
              authService.login("user", "password");
            }}
          >
            Login
          </Button>
        </Grid>
      </Box>
    </div>
  );
}
