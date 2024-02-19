import { Accordion, AccordionDetails, AccordionSummary, Box, Button, Container, Stack, TextField, Typography } from "@mui/material";
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import { useState } from 'react';

const IssuePage = () => {
    /** Responsible for editing issue */
    const [isEditMode, setIsEditMode] = useState(false);
    const [issueTitle, setIssueTitle] = useState('Issue Name');
    const [summary, setSummary] = useState('Summary');
    const handleEditClick = () => {
        setIsEditMode(!isEditMode);
    };

    /** Responsible for adding comments */
    const [comments, setComments] = useState<string[]>([]);
    const [currentComment, setCurrentComment] = useState('');
    const handleAddComment = () => {
        setComments([...comments, currentComment]);
        setCurrentComment('');
    };

    return (
        <Box>
            <Container component="main">
                {/* Holds issue name, number, and edit button */}
                <Box>
                    <Stack direction="row" spacing={2}> 
                        {/* Issue name, number, and author */}
                        <Box sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'flex-start',
                        }}>
                            {isEditMode ? (
                                <TextField
                                    value={issueTitle}
                                    onChange={(e) => setIssueTitle(e.target.value)}
                                    variant="outlined"
                                    fullWidth
                                />
                            ) : (
                                <Typography component="h4" variant="h4" margin="normal">{issueTitle}</Typography>
                            )}
                            <Typography component="h6" variant="h6">Issue #123 by Author</Typography>
                        </Box>
                       
                        {/* Placeholder to create space between name and button */}
                        <Typography variant="h6" style={{ flexGrow: 1 }}></Typography>

                        {/* Edit and Confirm Buttons */}
                        <Box sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'flex-end',
                        }}>
                            {isEditMode ? (
                                <Button variant="contained" onClick={handleEditClick}>Confirm</Button>
                            ) : (
                                <Button variant="contained" onClick={handleEditClick}>Edit</Button>
                            )}
                        </Box>
                    </Stack>
                </Box>

                {/* Underline Border */}
                <Box sx={{
                    height: '2px', 
                    backgroundColor: 'red', 
                    marginY: 2, 
                }}></Box>

                {/* Summary */}
                <Box>
                    {isEditMode ? (
                        <TextField 
                            value={summary}
                            label="Summary" 
                            variant="outlined" 
                            margin="normal" 
                            fullWidth  
                            multiline 
                            rows={6} 
                            onChange={(e) => setSummary(e.target.value)}/>
                            
                    ) : (
                        <TextField 
                        value={summary}
                        label="Summary" 
                        variant="outlined" 
                        margin="normal" 
                        fullWidth  
                        multiline 
                        rows={6} 
                        disabled/>
                    )}
                </Box>

                {/* Collapsible advanced options */}
                <Box>
                    <Accordion>
                        <AccordionSummary
                        expandIcon={<ExpandMoreIcon />}
                        aria-controls="panel1a-content"
                        id="panel1a-header"
                        >
                        <Typography>Advanced</Typography>
                        </AccordionSummary>

                        <AccordionDetails>
                            <Stack direction="column" spacing={2}>
                                <TextField label="Steps to Reproduce" variant="outlined" fullWidth  multiline rows={4}/>
                                <TextField label="Expected Results" variant="outlined" fullWidth multiline rows={4}/>
                                <TextField label="Actual Results" variant="outlined" fullWidth multiline rows={4}/>
                            </Stack>
                        </AccordionDetails>
                    </Accordion>
                </Box>

                {/* Underline Border */}
                <Box sx={{
                    height: '2px', 
                    backgroundColor: 'red', 
                    marginY: 2, 
                }}></Box>

                <Box>
                    {comments.map((comment, index) => (
                        <Box key={index}>
                             <TextField 
                                label={comment} 
                                variant="outlined" 
                                margin="normal" 
                                fullWidth  
                                multiline 
                                rows={4}
                                disabled>
                            </TextField>
                        </Box>
                    ))}
                </Box>

                {/* Area to add new comment and close issue */}
                <Box>
                    <Stack direction="column" spacing={2}>
                        {/* Comment */}
                        <Box>
                            <TextField 
                                label="Comment" 
                                variant="outlined" 
                                margin="normal" 
                                fullWidth  
                                multiline 
                                rows={4}
                                value={currentComment}
                                onChange={(e) => setCurrentComment(e.target.value)}/>
                        </Box>

                        {/* Button area */}
                        <Stack direction="row" spacing={2} justifyContent="right">
                            {/* Close Issue Button */}
                            <Box sx={{
                                display: 'flex',
                                flexDirection: 'column',
                            }}>
                                <Button variant="contained">Close Issue</Button>
                            </Box>

                            {/* Add Comment Button */}
                            <Box sx={{
                                display: 'flex',
                                flexDirection: 'column',
                            }}>
                                <Button variant="contained" onClick={handleAddComment}>Add</Button>
                            </Box>
                        </Stack>
                    </Stack>
                </Box>
            </Container>
        </Box>
    );
}

export default IssuePage;
