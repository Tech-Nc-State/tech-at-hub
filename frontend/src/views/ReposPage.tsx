import { useEffect, useState } from "react";
import {
  Box,
  Button,
  Checkbox,
  TextField,
  Typography,
  Container,
  Table,
  TableRow,
  TableCell,
  TableBody,
} from "@mui/material";
import { useSessionService } from "../services/SessionService";
import { SessionToken, login } from "../api/AuthApi";
import { useNavigate } from "react-router-dom";
import {
  CreateRepositoryForm,
  Repository,
  createRepository,
  getListing,
  getRepos
} from "../api/RepositoryApi";
import { User, getMe } from "../api/UserApi";

function ReposPage() {
  const sessionService = useSessionService();
  const navigate = useNavigate();
  const [repos, setRepos] = useState<Array<Repository>>([]);
  const [user, setUser] = useState<User | null>();

  const getReposHandler = async () => {
    let sessionToken: SessionToken = sessionService.getSessionToken();
    try {
      let user: User = await getMe(sessionToken);
      setUser(user);
      let newRepos: Repository[] = await getRepos(sessionToken, user.username);

      setRepos(newRepos);
      } catch (err) {}
  };
  
  useEffect(() => {
    getReposHandler();
  }, []);

  // const repos = [ ["tech@hub", "joey"], ["leetcode", "satan"]];


  return (
    <>
      <Box>
        <Typography sx={{ fontSize: "3rem", fontWeight: "700" }}>
          User's Repositories
        </Typography>
        <br />
        <Button href="/new" variant="contained">
          Create New Repository
        </Button>
        <br />
        <br />
        <br />
        <Box
          sx={{
            display: "flex",
            justifyContent: "center",
            // fontSize: "3rem",
            // fontWeight: "700",
            // height: "`500px",
            // width: "700px",
            // backgroundColor: "#FA455D",
            border: "2px solid black"
          }}
        >
          <Table sx={{margin: "30px" }}>
            <TableBody>
              {repos.map((repo) => (
                <><TableRow sx={{ border: "2px solid blue" }}>
                  <TableCell sx={{ borderLeft: "2px solid red", borderBottom: "2px solid red", borderTop: "2px solid red" }}>
                  <a href={"/repository/"+user?.username+"/"+repo.name}>                    
                    <Typography>{repo.name}</Typography>
                  </a>
                    <Typography>{""+user?.username}</Typography>
                  </TableCell>
                  <TableCell sx={{ textAlign: "right", borderRight: "2px solid red", borderBottom: "2px solid red", borderTop: "2px solid red" }}>
                    <img src="/src/assets/logo.svg" style={{ width: "75px", flexGrow: "0", marginRight: "10px", marginTop: "5px" }} />
                  </TableCell>
                </TableRow><br /></>
              ))}          
            </TableBody>
          </Table>
        </Box>
      </Box>
    </>
  );
}

export default ReposPage;

// width: 75px; flex-grow: 0; margin-right: 10px; margin-top: 5px;