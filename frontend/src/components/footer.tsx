import { Box, Paper, Typography, Button } from "@mui/material";
import TechAtLogo from "./logo";

function Footer() {
  return (
    <Box>
      <Paper
        elevation={10}
        sx={{
          width: "100%",
          height: 200,
          backgroundColor: "#FA455D",
          justifyContent: "center",
          alignItems: "center",
          textAlign: "center",
          p: 2,
        }}
      >
        <Typography variant="body1">
          Tech@Hub is a club project developed by Tech@NCSU.
        </Typography>

        <Box>
          <TechAtLogo />
        </Box>
        <Typography variant="caption">
          &copy; Copyright Tech@NCSU, 2023.
        </Typography>

        <Typography variant="caption">
          "Don't you dare steal this, it's copyrighted!" - Aditya
        </Typography>
      </Paper>
    </Box>
  );
}

export default Footer;
