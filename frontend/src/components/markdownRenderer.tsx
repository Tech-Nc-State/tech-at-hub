import React from "react";
import ReactMarkdown from "react-markdown";

interface markdownProps {
    text: string;
}

function renderMarkdown(props : markdownProps) {
    return <ReactMarkdown>{props.text}</ReactMarkdown>
}


export default renderMarkdown;
