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

function ReposPage() {
  return (
    <>
      <Box>
        <Typography sx={{ fontSize: "3rem", fontWeight: "700" }}>
          User's Repositories
        </Typography>
        <br />
        <Button
          sx={{
            fontSize: "2rem",
            fontWeight: "700",
            border: "solid 2px black",
            backgroundColor: "#FA455D",
            color: "white",
          }}
        >
          Add
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
            // height: "500px",
            // width: "700px",
            // backgroundColor: "#FA455D",
            border: "2px solid black"
          }}
        >
          <Table sx={{margin: "30px" }}>
            <TableBody>
              {/* {listing?.map((entry) => (
                <TableRow>
                  <TableCell>
                    <Typography>Name</Typography>
                  </TableCell>
                </TableRow>
              ))} */}
              <TableRow sx={{border: "2px solid blue"}}>
                <TableCell sx={{borderLeft: "2px solid red", borderBottom: "2px solid red", borderTop: "2px solid red"}}>
                  <Typography>Name</Typography>
                </TableCell>
                <TableCell sx={{textAlign: "right", borderRight: "2px solid red", borderBottom: "2px solid red", borderTop: "2px solid red"}}>
                  <img src="/src/assets/logo.svg" style={{width: "75px", flexGrow: "0", marginRight: "10px", marginTop: "5px"}} />
                </TableCell>
              </TableRow>
              <br />
              <TableRow sx={{border: "2px solid blue"}}>
                <TableCell sx={{borderLeft: "2px solid red", borderBottom: "2px solid red", borderTop: "2px solid red"}}>
                  <Typography>Name</Typography>
                </TableCell>
                <TableCell sx={{textAlign: "right", borderRight: "2px solid red", borderBottom: "2px solid red", borderTop: "2px solid red"}}>
                  <img src="/src/assets/logo.svg" style={{width: "75px", flexGrow: "0", marginRight: "10px", marginTop: "5px"}} />
                </TableCell>
              </TableRow>
              <br />
              <TableRow sx={{border: "2px solid blue"}}>
                <TableCell sx={{borderLeft: "2px solid red", borderBottom: "2px solid red", borderTop: "2px solid red"}}>
                  <Typography>Name</Typography>
                </TableCell>
                <TableCell sx={{textAlign: "right", borderRight: "2px solid red", borderBottom: "2px solid red", borderTop: "2px solid red"}}>
                  <img src="/src/assets/logo.svg" style={{width: "75px", flexGrow: "0", marginRight: "10px", marginTop: "5px"}} />
                </TableCell>
              </TableRow>
              <br />
              <TableRow sx={{border: "2px solid blue"}}>
                <TableCell sx={{borderLeft: "2px solid red", borderBottom: "2px solid red", borderTop: "2px solid red"}}>
                  <Typography>Name</Typography>
                </TableCell>
                <TableCell sx={{textAlign: "right", borderRight: "2px solid red", borderBottom: "2px solid red", borderTop: "2px solid red"}}>
                  <img src="/src/assets/logo.svg" style={{width: "75px", flexGrow: "0", marginRight: "10px", marginTop: "5px"}} />
                </TableCell>
              </TableRow>
              
              
            </TableBody>
          </Table>
        </Box>
      </Box>
    </>
  );
}

export default ReposPage;

// width: 75px; flex-grow: 0; margin-right: 10px; margin-top: 5px;