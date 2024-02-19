import { Avatar, Box, Button, Container, Stack, TextField, Typography } from "@mui/material";

const IssuePage = () => {
    return (
        <Box>
            <Container component="main">
                {/* Image and Title */}
                <Box sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                }}>
                    <Avatar alt="Tech@Logo" src="/assets/logo.svg"/>
                    <Typography component="h3" variant="h3">Issues</Typography>
                </Box>

                 {/* Underline Border */}
                 <Box sx={{
                    height: '2px', 
                    backgroundColor: 'red', 
                    marginY: 2, 
                }}></Box>

                <Box sx={{
                    display: 'flex',
                    flexDirection: 'column',
                }}>
                    <Button variant="contained">New</Button>
                </Box>
          
                {/* Holds issues */}
                <Box>
                    <Stack direction="column" spacing={2}> 
                        <TextField label="Issue 1" variant="outlined" margin="normal" fullWidth disabled/>
                        <TextField label="Issue 2" variant="outlined" margin="normal" fullWidth disabled/>
                        <TextField label="Issue 3" variant="outlined" margin="normal" fullWidth disabled/>
                        <TextField label="Issue 4" variant="outlined" margin="normal" fullWidth disabled/>
                        <TextField label="Issue 5" variant="outlined" margin="normal" fullWidth disabled/>
                    </Stack>
                </Box>
            </Container>
        </Box>
    );
}

export default IssuePage;
