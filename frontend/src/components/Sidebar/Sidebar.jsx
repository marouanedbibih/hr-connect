import React from "react";
import { Link, useLocation } from "react-router-dom";
import { BiUser } from "react-icons/bi";
import { FiUsers } from "react-icons/fi";
import { GoOrganization } from "react-icons/go";
import MenuLink from "../Link/MenuLink";
import { SiAwsorganizations } from "react-icons/si";
import { MdWork } from "react-icons/md";
import { PiUsersFourFill } from "react-icons/pi";




import { useStateContext } from "../../context/ContextProvider";

function Sidebar() {
  const location = useLocation();
  const { user, token, role } = useStateContext();
  const roleInt = parseInt(role);

  const isActive = (route) => {
    return location.pathname === route ? "bg-zinc-300 bg-opacity-50" : "";
  };

  return (
    <aside className="w-60 bg-violet-800 px-4">
      <div className="h-20 flex items-center">
        <div className="text-white text-lg font-bold font-['Roboto'] leading-normal">
          Dashboard
        </div>
      </div>

      {token && (
        <div>
          {role === "admin" && (
            <>
            <MenuLink route="/admins" label="Admins" icon={<BiUser color="white" />} top_vl="0" />
            <MenuLink route="/employees" label="Employees" icon={<PiUsersFourFill color="white" />} top_vl="40px" />
            <MenuLink route="/companies" label="Companies" icon={<GoOrganization color="white" />} top_vl="40px" />
            <MenuLink route="/departes" label="Departes" icon={<SiAwsorganizations color="white" />} top_vl="40px" />
            <MenuLink route="/jobs" label="Jobs" icon={<MdWork color="white" />} top_vl="40px" />
          </>

          )}
          {role === "candidate"  && (
            <MenuLink route="/resume" label="Resume" icon={<FiUsers color="white" />} top_vl="40px" />
          )}

        </div>
      )}
    </aside>
  );
}

export default Sidebar;
