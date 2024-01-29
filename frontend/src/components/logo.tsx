import { Typography, Box } from "@mui/material";
import logoSvg from "../assets/logo.svg";

function TechAtLogo() {
  return (
    <Box
      // display="flex"
      // justifyContent="start"
      // alignItems="center"
      margin="0px"
    >
      <img
        src={logoSvg}
        style={{ width: "75px", flexGrow: 0, marginRight: "10px", marginTop: "5px" }}
      />
      {/* <Typography
        variant="h6"
        noWrap
        sx={{
          display: "flex",
          fontWeight: 700,
          letterSpacing: ".1rem",
          color: "inherit",
          textDecoration: "none",
          alignItems: "center",
          flexGrow: 0,
        }}
      >
        Tech @ Hub
      </Typography> */}
    </Box>
  );
}

export default TechAtLogo;
