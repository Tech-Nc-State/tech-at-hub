import * as React from "react";

import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Typography,
} from "@mui/material";
import { MovieDetails } from "../routes/search";

interface ListTableProps {
  movies: MovieDetails[];
}

const headers = ["", "Title", "Release Date", "Overview", "Genres", "TMDB ID"];

export default function ListTable({ movies }: ListTableProps) {
  return (
    <TableContainer component={Paper} sx={{ borderRadius: 5 }}>
      <Table sx={{ minWidth: 650 }} aria-label="simple table">
        <TableHead>
          <TableRow sx={{ backgroundColor: "#F21B42" }}>
            {headers.map((header) => (
              <TableCell>
                <Typography
                  variant="h6"
                  sx={{ fontWeight: 700, color: "white" }}
                >
                  {header}
                </Typography>
              </TableCell>
            ))}
          </TableRow>
        </TableHead>
        <TableBody>
          {movies != null &&
            movies.map((movie) => {
              const genres = movie.genres.join(", ");
              return (
                <TableRow
                  key={movie.title}
                  sx={{ "&:last-child td, &:last-child th": { border: 0 } }}
                >
                  <TableCell>
                    <img src={movie.poster_url} style={{ width: 100 }} />
                  </TableCell>
                  <TableCell>
                    <Typography sx={{ fontWeight: 700 }}>
                      {movie.title}
                    </Typography>
                  </TableCell>
                  <TableCell>{movie.release_date}</TableCell>
                  <TableCell>{movie.overview}</TableCell>
                  <TableCell>{genres}</TableCell>
                  <TableCell>{movie.id}</TableCell>
                </TableRow>
              );
            })}
        </TableBody>
      </Table>
    </TableContainer>
  );
}
