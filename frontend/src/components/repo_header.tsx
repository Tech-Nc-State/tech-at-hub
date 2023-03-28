import {Box, Breadcrumbs, Link, Chip} from "@mui/material"

export interface RepositoryHeaderProps {
    initialPos?: number;
    repo_org: string,
    //#org
    org_link: string,
    repo_name: string,
    // #repo
    repo_link: string,
    repo_type: string
}

export function RepositoryHeader(props: RepositoryHeaderProps) {
    return (
        <Box sx={{ width: "100%", paddingTop: "16px" }}>
            <Box sx={{ paddingLeft: "32px", paddingRight: "32px", display: "flex", alignItems: "flex-end" }}>
                <Breadcrumbs separator="/">
                    <Link underline="hover" href={props.org_link}>
                        {props.repo_org}
                    </Link>
                    <Link underline="hover" href={props.repo_link}>
                        {props.repo_name}
                    </Link>
                </Breadcrumbs>
                <Chip label={props.repo_type} size="small" variant="outlined" sx={{ marginLeft: "8px", fontSize: "70%" }}/>
            </Box>
        </Box>
    );
}
