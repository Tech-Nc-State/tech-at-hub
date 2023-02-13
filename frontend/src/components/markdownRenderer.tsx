
import React from 'react';
import ReactMarkdown from 'react-markdown';


function markdownRender(input : string) {
    return (<ReactMarkdown children={input}/>);
}

export default markdownRender;