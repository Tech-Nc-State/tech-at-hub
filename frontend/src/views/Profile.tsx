import React, { useState, useEffect } from 'react';
import { User, getMe, getProfilePicture } from "../api/UserApi";
import { useSessionService } from "../services/SessionService";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faRightFromBracket, faRightToBracket, faUserPlus } from "@fortawesome/free-solid-svg-icons";
import CircularProgress from '@mui/material/CircularProgress';
import { Description } from '@mui/icons-material';
import { Box, Avatar, Button, Checkbox, TextField, Typography } from "@mui/material";
import ProfileRender from "../components/profile";
import { useNavigate } from "react-router-dom";


// Assuming pfps import is used elsewhere or can be removed if not needed
// import pfps from "../../sampleData/Users.js";

function Profile() {
    const navigate = useNavigate();

    const editProfile = () => {
        navigate("/edit-profile");
        window.location.reload();
    };

    
    const [Userprofile, setProfile] = useState({
        pfp: '',
        username: '',
        id: 0,
        description: '',
        repositories: ["test1"]
    });
    const sessionService = useSessionService();

    const sample = {
        id: 1,
        username: "sampleUsername",
        email: "sample@email.com",
        pfp: "src/assets/logo.svg",
        firstName: "John",
        lastName: "Doe",
        description: "This is a sample user description.",
        birthDate: new Date("1990-01-01")
    };

    useEffect(() => {
        async function fetchProfilePicture() {
            try {
                // authorize session and retreive user info
                let sessionToken = sessionService.getSessionToken();
                const user = await getMe(sessionToken);
                if(user != null) {
                    // get user information
                        if (sessionService.hasSessionToken()) {
                            getMe(sessionService.getSessionToken()).then((user) => {
                                getProfilePicture(user.username).then((picture) => {
                                const base64 = picture.reduce((data, byte) => data + String.fromCharCode(byte), "");
                                setProfile( prevState => ({
                                    ...prevState, 
                                    pfp: base64
                                })
                            );
                                });
                            });
                            }
                    // let pfp = new TextDecoder().decode(await getProfilePicture(user.username));
                    let usrname = user.username;
                    let desc = user.description;
                    let userId = user.id;

                    // set state
                    setProfile( prevState => ({
                        ...prevState,
                        // pfp: pfp,
                        username: usrname,
                        id: userId,
                        description: desc
                    }));
                } else {
                    console.error("not logged in");
                }
            } catch (error) {
                console.error("Failed to fetch profile:", error);
                // Handle the error appropriately
            }
        }

        fetchProfilePicture();
    }, [sessionService]);

    return (
        <Box className="ProfileWrapper">
            <Box  sx={{
                width: 1, 
                display: 'flex',
                alignItems: 'center',
                flexWrap: "nowrap",
                whiteSpace:"nowrap",
                pl: 5
                }}>

                <Box className="profilePic" sx={{width: 1, mr:10}}>
                    <Avatar alt="" src={Userprofile.pfp} sx={{width:200, height: 200}} />
                </Box>

                <Box className="userName">
                    <h1> {Userprofile.username ? Userprofile.username : "User's"} profile</h1>
                </Box>

                <Box sx={{ml: 25}}>
                    <Button variant="contained" className="editProfile" onClick={editProfile}>Edit Profile</Button>
                </Box>
            </Box>


            <Box className="description">
                    <Box>
                        <Typography variant='h6'>
                            Description
                        </Typography>
                        <Typography variant='body2'>
                            {Userprofile.description ? Userprofile.description : "No Description"}
                        </Typography>
                    </Box>
                   <Box sx={{mt: 5}}>
                    <Typography variant='h6'>
                            Repos
                        </Typography>
                            TODO: add REPO stuff Sheil is working on
                        {/* USE REPO COMPONENT OTHER TEAM IS MAKING */}
                   </Box>
                   

            </Box>
        </Box>
    );
}

export default Profile;
