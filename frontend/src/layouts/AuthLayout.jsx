import React from "react";
import { Outlet } from "react-router-dom";
import { useStateContext } from "../context/ContextProvider";
import { Navigate } from "react-router-dom";

function GuestLayout() {
  const { user, token, role } = useStateContext();

  if (token) {
    if (role === "admin") {
      return <Navigate to="/admins" />;
    } else if (role === "candidate") {
      return <Navigate to="/resume" />;
    }
  }
  return (
    <div className="bg-light flex flex-col h-screen">
      <div className="w-full h-20 relative bg-white shadow" />
      <div className="w-full flex-1 flex justify-center items-center">
        <Outlet />
      </div>
    </div>
  );
}

export default GuestLayout;
