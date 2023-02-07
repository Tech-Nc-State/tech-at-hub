import { useState } from "react";
import { Button, TextField } from "@mui/material";
import SearchIcon from "@mui/icons-material/Search";
import { Form } from "react-router-dom";

interface SearchBarProps {
  search: (snippet: string) => void;
}

function SearchBar(props: SearchBarProps) {
  const [snippet, setSnippet] = useState("");

  return (
    <div>
      <TextField
        id="search-bar"
        className="text"
        onInput={(e: any) => {
          setSnippet(e.target.value as string);
        }}
        label="Search for a Movie"
        variant="outlined"
        placeholder="Search..."
        size="small"
        name="snippet"
      />
      <Button
        variant="contained"
        color="secondary"
        onClick={() => props.search(snippet)}
        sx={{ ml: 1 }}
      >
        <SearchIcon />
      </Button>
    </div>
  );
}

export default SearchBar;
