import * as React from "react";

import {
  AppBar,
  Box,
  Toolbar,
  IconButton,
  Typography,
  Menu,
  Container,
  Avatar,
  Button,
  Tooltip,
  MenuItem,
} from "@mui/material";

import { Link } from "react-router-dom";

import MenuIcon from "@mui/icons-material/Menu";
import AdbIcon from "@mui/icons-material/Adb";
import TechAtLogo from "./logo";
import { useSessionService } from "../services/SessionService";
import { useEffect, useState } from "react";
import { getProfilePicture, getMe } from "../api/UserApi";

const pages = [{ name: "About", route: "/about" }];
const settings = ["Profile", "Account", "Dashboard", "Logout"];

function NavBar() {
  const sessionService = useSessionService();
  const [pfp, setPfp] = useState<any>(null);
  const [username, setUsername] = useState<any>("Not Logged In");

  useEffect(() => {
    if (sessionService.hasSessionToken()) {
      getMe(sessionService.getSessionToken()).then((user) => {
        setUsername(user.username)
        getProfilePicture(user.username).then((picture) => {
          const base64 = btoa(
            picture.reduce((data, byte) => data + String.fromCharCode(byte), "")
          );
          setPfp(base64);
        });
      });
    }
  }, []);

  const [anchorElNav, setAnchorElNav] = React.useState<null | HTMLElement>(
    null
  );
  const [anchorElUser, setAnchorElUser] = React.useState<null | HTMLElement>(
    null
  );

  const handleOpenNavMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorElNav(event.currentTarget);
  };
  const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorElUser(event.currentTarget);
  };

  const handleCloseNavMenu = () => {
    setAnchorElNav(null);
  };

  const handleCloseUserMenu = () => {
    setAnchorElUser(null);
  };

  return (
    <AppBar position="sticky">
      <Container maxWidth="xl">
        <Toolbar disableGutters>
          {/* Main Size */}
          {/* Logo */}
          <Box sx={{ display: { xs: "none", md: "flex" } }}>
            <Link to="/" style={{ textDecoration: "none", color: "white" }}>
              <TechAtLogo />
            </Link>
          </Box>

          {/* Buttons */}
          <Box sx={{ flexGrow: 1, display: { xs: "none", md: "flex" } }}>
            {pages.map(({ name, route }) => (
              <Link key={name} to={route} style={{ textDecoration: "none" }}>
                <Button
                  key={name}
                  onClick={handleCloseNavMenu}
                  sx={{ m: 1, display: "block" }}
                  variant="contained"
                  color="secondary"
                >
                  {name}
                </Button>
              </Link>
            ))}
          </Box>

          <Box sx={{ flexGrow: 1, display: { xs: "flex", md: "none" } }}>
            <IconButton
              size="large"
              aria-label="account of current user"
              aria-controls="menu-appbar"
              aria-haspopup="true"
              onClick={handleOpenNavMenu}
              color="inherit"
            >
              <MenuIcon />
            </IconButton>
            <Menu
              id="menu-appbar"
              anchorEl={anchorElNav}
              anchorOrigin={{
                vertical: "bottom",
                horizontal: "left",
              }}
              keepMounted
              transformOrigin={{
                vertical: "top",
                horizontal: "left",
              }}
              open={Boolean(anchorElNav)}
              onClose={handleCloseNavMenu}
              sx={{
                display: { xs: "block", md: "none" },
              }}
            >
              {pages.map(({ name, route }) => (
                <Link
                  key={name}
                  to={route}
                  style={{ color: "black", textDecoration: "none" }}
                >
                  <MenuItem key={name} onClick={handleCloseNavMenu}>
                    <Typography textAlign="center">{name}</Typography>
                  </MenuItem>
                </Link>
              ))}
            </Menu>
          </Box>

          <Box sx={{ display: { xs: "flex", md: "none" }, flexGrow: 1 }}>
            <Link to="/" style={{ textDecoration: "none", color: "white" }}>
              <TechAtLogo />
            </Link>
          </Box>
          
          <Box sx={{ margin: "10px"}}>
            {username}
          </Box>
          <Box sx={{ flexGrow: 0 }}>
            <Tooltip title="Open settings">
              <IconButton onClick={handleOpenUserMenu} sx={{ p: 0 }}>
                <Avatar alt="" src={`data:;base64,${pfp}`} />
              </IconButton>
            </Tooltip>
            <Menu
              sx={{ mt: "45px" }}
              id="menu-appbar"
              anchorEl={anchorElUser}
              anchorOrigin={{
                vertical: "top",
                horizontal: "right",
              }}
              keepMounted
              transformOrigin={{
                vertical: "top",
                horizontal: "right",
              }}
              open={Boolean(anchorElUser)}
              onClose={handleCloseUserMenu}
            >
              {settings.map((setting) => (
                <MenuItem key={setting} onClick={handleCloseUserMenu}>
                  <Typography textAlign="center">{setting}</Typography>
                </MenuItem>
              ))}
            </Menu>
          </Box>
        </Toolbar>
      </Container>
    </AppBar>
  );
}
export default NavBar;
