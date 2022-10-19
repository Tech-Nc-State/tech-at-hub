import Textfield from "@mui/material/TextField";
import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import Button from "@mui/material/Button";

export default function LoginPage() {
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
          <Button variant="contained" size="large" color="secondary">
            Login
          </Button>
        </Grid>
      </Box>
    </div>
  );
}
