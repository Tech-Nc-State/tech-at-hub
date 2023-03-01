import * as React from "react";
import {styled} from "@mui/material/styles"
import {Box, Tabs, Tab, Typography, Breadcrumbs, Link, TabProps, Chip} from "@mui/material"

const StyledTab = styled((props: TabProps) => (<Tab disableRipple {...props}/>))(({theme}) => ({
    textTransform: 'none',

}));

interface RepositoryHeaderProps {
    initialPos?: number;
}

const defaultProps = {
    initialPos: 0
}

function RepositoryHeader(_props: RepositoryHeaderProps) {

    const repo_org = "Tech-Nc-State";
    const org_link = "#org";
    const repo_name = "tech-at-hub";
    const repo_link = "#repo";
    const repo_type = "public";

    const props = {...defaultProps, ..._props};
    const [value, set_value] = React.useState(props.initialPos);

    const swap_component = (event: React.SyntheticEvent, new_value: number) => {
        set_value(new_value);
    }

    return (
        <Box sx={{ width: "100%", paddingTop: "16px" }}>
            <Box sx={{ paddingLeft: "32px", paddingRight: "32px", display: "flex", alignItems: "flex-end" }}>
                <Breadcrumbs separator="/">
                    <Link underline="hover" href={org_link}>
                        {repo_org}
                    </Link>
                    <Link underline="hover" href={repo_link}>
                        {repo_name}
                    </Link>
                </Breadcrumbs>
                <Chip label={repo_type} size="small" variant="outlined" sx={{ marginLeft: "8px", fontSize: "70%" }}/>
            </Box>
            <Box sx={{ borderBottom: "1px solid #d2d2d2", paddingLeft: "32px", paddingRight: "32px" }}>
                <Tabs value={value} onChange={swap_component}>
                    <StyledTab label="Code"/>
                    <StyledTab label="Settings"/>
                </Tabs>
            </Box>
        </Box>
    );
}

export default RepositoryHeader;