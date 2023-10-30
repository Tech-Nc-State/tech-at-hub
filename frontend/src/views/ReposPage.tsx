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
        {/* <Box
          sx={{
            display: "flex",
            justifyContent: "center",
            fontSize: "3rem",
            fontWeight: "700",
            height: "500px",
            width: "700px",
            backgroundColor: "#FA455D",
          }}
        ></Box> */}
        <Table>
          <TableBody>
            {/* {listing?.map((entry) => (
              <TableRow>
                <TableCell>
                  <Typography>Name</Typography>
                </TableCell>
              </TableRow>
            ))} */}
            <TableRow>
              <TableCell>
                <Typography>Name</Typography>
              </TableCell>
            </TableRow>
          </TableBody>
        </Table>
      </Box>
    </>
  );
}

export default ReposPage;
