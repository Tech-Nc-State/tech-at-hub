import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";

import { createBrowserRouter, RouterProvider } from "react-router-dom";
import HomePage from "./views/HomePage";
import LoginPage from "./views/LoginPage";
import SignupPage from "./views/SignupPage";
import RepositoryPage from "./views/RepositoryPage";

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
        path: "/about",
        element: <p>Hi there!</p>,
      },
      {
        path: "/login",
        element: <LoginPage />
      },
      {
        path: "signup",
        element: <SignupPage />
      },
      {
        path: "repository",
        element: <RepositoryPage />
      }
    ],
  },
]);

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>
);
