import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import MarkdownRender from "./components/markdownRenderer";

import { createBrowserRouter, RouterProvider } from "react-router-dom";
import renderMarkdown from "./components/markdownRenderer";
import { render } from "react-dom";


const ting = `
# h1 tag for fun
A paragraph with *emphasis* and **strong importance**. 

> A block quote with ~strikethrough~ and a URL: https://reactjs.org.

* Lists
* [ ] todo
* [x] done
`

const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      {
        index: true,
        // element: <HomePage />,
      },
      {
        path: "/about",
        element: <p>Hi there!</p>,
      },
      {
          path: "/markdown-test",
          element: <MarkdownRender text = {ting}></MarkdownRender>,
      },
    ],
  },
]);

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>
);
