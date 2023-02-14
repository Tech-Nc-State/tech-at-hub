
import React from 'react';
import ReactMarkdown from 'react-markdown';


function MarkdownRender(props) {
    return (<ReactMarkdown children={props.markdown}/>);
}

export default markdownRender;