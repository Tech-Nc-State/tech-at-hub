import {
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Paper
  } from "@mui/material";

  function createData(
    path: String,
    lastCommit: String,
    lastCommitDate: String
  );

  const mockData = [
createData('Tech@HubAPI', 'Update Tech@HubAPITest.csproj', '3 weeks ago'),
createData('Tech@HubAPITest', 'Update Tests cases', '1 day ago'),
createData('frontend', 'added header component', '3 hours ago'),

  ];

  function repositoryFile() {

    return(
        <DataGrid
  rows={rows}
  columns={columns}
  pageSize={5}
  rowsPerPageOptions={[5]}
  checkboxSelection
/>
    )
  }