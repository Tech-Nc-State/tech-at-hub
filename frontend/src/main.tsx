import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";

import { createBrowserRouter, RouterProvider } from "react-router-dom";
import HomePage from "./views/HomePage";
import LoginPage from "./views/LoginPage";
import SignupPage from "./views/SignupPage";
import IssuesPage from "./views/IssuesPage"
import IssuePage from "./views/IssuePage";
import RepositoryPage from "./views/RepositoryPage";
import CreateRepositoryPage from "./views/CreateRepositoryPage";
import ReposPage from "./views/ReposPage";

const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      {
        index: true,
        element: <HomePage />,
      },
      {
        path: "about",
        element: <p>Hi there!</p>,
      },
      {
        path: "login",
        element: <LoginPage />
      },
      {
        path: "signup",
        element: <SignupPage />
      },
      {
        path: "issues",
        element: <IssuesPage />
      },
      {
        path: "issue",
        element: <IssuePage />
      },
      {
        path: "login",
        element: <LoginPage />,
      },
      {
        path: "signup",
        element: <SignupPage />,
      },
      {
        path: "repository/:username/:repoName",
        element: <RepositoryPage />,
        loader: ({ params }) => {
          return params;
        },
      },
      {
        path: "new",
        element: <CreateRepositoryPage />,
      },
      {
        path: "repos",
        element: <ReposPage />,
      },
    ],
  },
]);

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>
);
