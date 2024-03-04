import * as React from 'react';
import Avatar from '@mui/material/Avatar';
import { User } from "../api/UserApi";
import "./style.css"
import Button from '@mui/material/Button';
import Box from "@mui/material/Box";
import CircularProgress from '@mui/material/CircularProgress';
import { useNavigate } from "react-router-dom";
import { Typography } from '@mui/material';

function ProfileRender( props: any ) {
    const navigate = useNavigate();

    const editProfile = () => {
        navigate("/edit-profile");
        window.location.reload();
    };
    let user = props.user;

    return (
        <Box className="ProfileWrapper">
            <Box  sx={{
                width: 1, 
                display: 'flex',
                alignItems: 'center',
                flexWrap: "nowrap",
                pl: 5
                }}>

                <Box className="profilePic">
                    {user.pfp ? 
                    <Avatar alt="" src={user.pfp} sx={{width:200, height: 200}} /> : 
                    <CircularProgress/>}
                </Box>

                <Box className="userName">
                    <h1> {user.username ? user.username : "User's"} profile</h1>
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
                            {user.description ? user.description : "No Description"}
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

export default ProfileRender;