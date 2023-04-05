import { Box, Button, Checkbox, TextField, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { useSessionService } from "../services/SessionService";
import { SessionToken, login } from "../api/AuthApi";
import { useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faLock, faUser } from "@fortawesome/free-solid-svg-icons";
import {
  CreateRepositoryForm,
  Repository,
  createRepository,
} from "../api/RepositoryApi";
import { User, getMe } from "../api/UserApi";

function CreateRepositoryPage() {
  const sessionService = useSessionService();
  const navigate = useNavigate();
  const [repoInfo, setRepoInfo] = useState(new CreateRepositoryForm());

  const createHandler = async () => {
    let sessionToken: SessionToken = sessionService.getSessionToken();
    try {
      // use the API to create the repo
      let repository: Repository = await createRepository(
        sessionToken,
        repoInfo
      );
      // redirect to the repo page
      let user: User = await getMe(sessionToken);
      navigate(`/repository/${user.username}/${repository.name}`);
    } catch (err) {}
  };

  return (
    <Box>
      <Typography sx={{ fontSize: "3rem", fontWeight: "700" }}>
        Create Repository
      </Typography>
      <Box sx={{ display: "flex", justifyContent: "center" }}>
        <Box
          sx={{
            display: "flex",
            flexDirection: "column",
            width: "50%",
            alignItems: "center",
          }}
        >
          <Box sx={{ display: "flex", alignItems: "center" }}>
            <FontAwesomeIcon icon={faUser} />
            <TextField
              id="standard-basic"
              label="Repository Name"
              value={repoInfo.name}
              onChange={(event) => {
                setRepoInfo({
                  ...repoInfo,
                  name: event.target.value,
                });
              }}
              sx={{ m: "10px" }}
            />
          </Box>

          <Box sx={{ display: "flex", alignItems: "center" }}>
            <FontAwesomeIcon icon={faLock} style={{}} />
            <Checkbox
              id="standard-basic"
              value={repoInfo.isPublic}
              onChange={(event) => {
                setRepoInfo({
                  ...repoInfo,
                  isPublic: event.target.value == "on", // TODO: This doesn't work
                });
              }}
              sx={{ m: "10px" }}
            />
          </Box>
          <Button variant="contained" onClick={createHandler}>
            Create
          </Button>
        </Box>
      </Box>
    </Box>
  );
}

export default CreateRepositoryPage;
