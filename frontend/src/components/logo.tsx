import { Typography, Box } from "@mui/material";
// import popcornSvg from "../assets/popcorn.svg";

function TechAtLogo() {
  return (
    <Box
      display="flex"
      justifyContent="start"
      alignItems="center"
      margin="10px"
    >
      {/* <img
        src={""}
        style={{ width: "50px", flexGrow: 0, marginRight: "10px" }}
      /> */}
      <Typography
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
      </Typography>
    </Box>
  );
}

export default TechAtLogo;
