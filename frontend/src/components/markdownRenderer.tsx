import React from "react";
import ReactMarkdown from "react-markdown";
import remarkGfm from 'remark-gfm';

interface markdownProps {
    text: string;
}

function renderMarkdown(props : markdownProps) {
    return <ReactMarkdown children = {props.text} remarkPlugins={[remarkGfm]}/>
}


export default renderMarkdown;
