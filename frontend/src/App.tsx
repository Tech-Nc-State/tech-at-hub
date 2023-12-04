import { Box, CssBaseline, Paper } from "@mui/material";
import { Outlet } from "react-router-dom";

import { createTheme, ThemeProvider } from "@mui/material/styles";

import NavBar from "./components/navbar";
import Footer from "./components/footer";
import { GlobalServices } from "./services/GlobalServices";
import React from "react";

const theme = createTheme({
  palette: {
    primary: {
      main: "#CC0000",
    },
    secondary: {
      main: "#FFA0A0",
    },
  },
  typography: {
    fontFamily: "Oxygen",
    h1: {
      fontFamily: "Oxygen",
    },
  },
});

function App() {
  return (
    <GlobalServices>
      <Box>
        <ThemeProvider theme={theme}>
          <CssBaseline />
          <NavBar />
          <Box
            sx={{
              display: "flex",
              justifyContent: "center",
              alignItems: "center",
              p: 3,
            }}
          >
            <Box
              sx={{
                minWidth: 700,
                minHeight: "70vh",
                justifyContent: "center",
                alignItems: "center",
                textAlign: "center",
                p: 5,
              }}
            >
              <Outlet />
            </Box>
          </Box>
          <Footer />
        </ThemeProvider>
      </Box>
    </GlobalServices>
  );
}

export default App;
