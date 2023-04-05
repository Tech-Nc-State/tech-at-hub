import { styled } from "@mui/material/styles";
import {
  RepositoryHeader,
  RepositoryHeaderProps,
} from "../components/repo_header";
import { Box, Tabs, Tab, TabProps, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { useLoaderData } from "react-router";
import { useSessionService } from "../services/SessionService";
import { getBranches } from "../api/RepositoryApi";

const StyledTab = styled((props: TabProps) => <Tab disableRipple {...props} />)(
  ({ theme }) => ({
    textTransform: "none",
  })
);

function RepositoryPage() {
  const [tab, setTab] = useState(0);
  const repoInfo: any = useLoaderData();
  const sessionService = useSessionService();
  const [branches, setBranches] = useState();

  const swapTab = (event: React.SyntheticEvent, new_value: number) => {
    setTab(new_value);
  };

  useEffect(() => {
    getBranches(
      sessionService.getSessionToken(),
      repoInfo.username,
      repoInfo.repoName
    ).then((response) => {
      setBranches(response.data);
    });
  }, []);

  var repositoryProps: RepositoryHeaderProps = {
    repo_link: "/",
    repo_name: repoInfo.repoName,
    repo_org: repoInfo.username,
    repo_type: "public",
    org_link: "/",
  };

  return (
    <Box>
      <RepositoryHeader {...repositoryProps} />
      <Typography>{`Branches: ${branches}`}</Typography>
      <Box
        sx={{
          borderBottom: "1px solid #d2d2d2",
          paddingLeft: "32px",
          paddingRight: "32px",
        }}
      >
        <Tabs value={tab} onChange={swapTab}>
          <StyledTab label="Code" />
          <StyledTab label="Settings" />
        </Tabs>
      </Box>
    </Box>
  );
}

export default RepositoryPage;
