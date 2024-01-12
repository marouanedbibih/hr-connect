import React from "react";
import { Outlet, Navigate } from "react-router-dom";
import { useStateContext } from "../context/ContextProvider";
import Sidebar from "../components/Sidebar/Sidebar";
import Navbar from "../components/Navbar/Navbar";

function CandidateLayout() {
    const { user, token, role, setUser, _setToken, _setRole,notification } = useStateContext();
    if (!token || (role !== "admin")) {
      return <Navigate to="/login" />;
    }
  
    return (
      <div className="flex min-h-screen">
        <Sidebar />
        <div className="flex-1">
          <Navbar  />
          <main className="px-16">
            <Outlet />
          </main>
          {notification &&
            <div className="notification">
              {notification}
            </div>
          }
        </div>
      </div>
    );
}

export default CandidateLayout