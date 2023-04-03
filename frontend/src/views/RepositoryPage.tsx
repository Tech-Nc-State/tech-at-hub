import {styled} from "@mui/material/styles"
import { RepositoryHeader, RepositoryHeaderProps } from "../components/repo_header";
import {Box, Tabs, Tab, TabProps} from "@mui/material"
import {useState} from 'react'

const StyledTab = styled((props: TabProps) => (<Tab disableRipple {...props}/>))(({theme}) => ({
    textTransform: 'none',
}));

function RepositoryPage() {

    const [tab, setTab] = useState(0);

    const swapTab = (event: React.SyntheticEvent, new_value: number) => {
        setTab(new_value);
    }

    var repositoryProps: RepositoryHeaderProps = {
        repo_link: "/",
        repo_name: "tech-at-hub",
        repo_org: "Tech@NCSU",
        repo_type: "public",
        org_link: "/"
    };

    return (
        <Box>
            <RepositoryHeader {...repositoryProps} />
            <Box sx={{ borderBottom: "1px solid #d2d2d2", paddingLeft: "32px", paddingRight: "32px" }}>
                <Tabs value={tab} onChange={swapTab}>
                    <StyledTab label="Code"/>
                    <StyledTab label="Settings"/>
                </Tabs>
            </Box>
        </Box>
    );
}

export default RepositoryPage;
