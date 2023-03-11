import { Box, Button, TextField } from "@mui/material";
import { redirect } from "react-router-dom";
import axios from "axios";
import { useUserService } from "../services/UserService";
import { setToken, getAuthHeader } from "../api/TokenAPI";
import {useEffect, useState } from "react";

function LoginPage() {
    const userService = useUserService();
    const [loginInfo, setLoginInfo] = useState({username: '', password: ''});

    useEffect(() => {
        if (userService.hasUser()) {
            redirect("/");
        }
    }, []);

    function login(username: string, password: string) {
        axios
            .post("http://localhost:5000/auth/login", {
                username: username,
                password: password
            }).then((res) => {
                setToken({
                    token: res.data.token,
                    expiration: res.data.expiration
                });
                axios.get("http://localhost:5000/user/me", {
                    headers: getAuthHeader()
                }).then((res) => {
                    userService.setCurrentUser(res.data);
                }).catch((error) => {
                    console.log(error)
                })
            }).catch((error) => {
                console.log(error);
                alert(error);
        });
    }

    return (
        <Box>
            <p>Login Page</p>
            <TextField
                id="standard-basic"
                label="Username"
                value={loginInfo.username}
                onChange={(event) => {
                    setLoginInfo({
                        ...loginInfo,
                        username: event.target.value
                    });
                }}
            />
            <TextField
                id="standard-basic"
                label="Password"
                type="password"
                value={loginInfo.password}
                onChange={(event) => {
                    setLoginInfo({
                        ...loginInfo,
                        password: event.target.value
                    });
                }}
            />
            <Button
                variant="contained"
                onClick={() => login(loginInfo.username, loginInfo.password)}
            >
                Login
            </Button>
        </Box>
    );
}

export default LoginPage;
