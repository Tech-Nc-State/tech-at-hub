import { styled } from "@mui/material/styles";
import {
  RepositoryHeader,
  RepositoryHeaderProps,
} from "../components/repo_header";
import {
  Box,
  Tabs,
  Tab,
  TabProps,
  Typography,
  Table,
  TableBody,
  TableRow,
  TableCell,
} from "@mui/material";
import { useEffect, useState } from "react";
import { useLoaderData } from "react-router";
import { useSessionService } from "../services/SessionService";
import {
  Branch,
  DirectoryEntry,
  getBranches,
  getListing,
} from "../api/RepositoryApi";
import { faFile, faFolder } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

const StyledTab = styled((props: TabProps) => <Tab disableRipple {...props} />)(
  ({ theme }) => ({
    textTransform: "none",
  })
);

function RepositoryPage() {
  const [tab, setTab] = useState(0);
  const repoInfo: any = useLoaderData();
  const sessionService = useSessionService();
  const [listing, setListing] = useState<DirectoryEntry[]>();

  const swapTab = (event: React.SyntheticEvent, new_value: number) => {
    setTab(new_value);
  };

  useEffect(() => {
    getListing(
      sessionService.getSessionToken(),
      repoInfo.username,
      repoInfo.repoName,
      "master",
      ""
    ).then((dirs) => {
      setListing(dirs);
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
      <Table>
        <TableBody>
          {listing?.map((entry) => (
            <TableRow>
              <TableCell>
                <FontAwesomeIcon icon={entry.isDirectory ? faFolder : faFile} />
                <Typography>{entry.name}</Typography>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
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
