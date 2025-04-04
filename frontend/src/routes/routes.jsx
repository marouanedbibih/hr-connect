import { createBrowserRouter } from "react-router-dom";
import AuthLayout from "../layouts/AuthLayout";
import Signup from "../views/auth/Signup";

// Admin Layout & View
import AdminLayout from "../layouts/AdminLayout";
import AdminList from "../views/admin/AdminList";
import AdminForm from "../views/admin/AdminForm";

// Company Layout & View
import CompanyLayout from "../layouts/CompanyLayout";
import Companies from "../views/companies/Companies";
import DepartesLayout from "../layouts/DepartesLayout";
import DepartesList from "../views/departes/DepartesList";
import DepartesForm from "../views/departes/DepartesForm";
import CompanyDepartes from "../views/companies/CompanyDepartes";
import JobLayout from "../layouts/JobLayout";
import JobList from "../views/job/JobList";
import JobForm from "../views/job/jobForm";
import CompanyDepartementInfos from "../views/companies/CompanyDepartementInfos";
import EmployeeLayout from "../layouts/EmployeeLayout";
import EmployeeList from "../views/employee/EmployeeList";
import EmployeeForm from "../views/employee/EmployeeForm";
import Login from "../views/auth/Login";


const routes = createBrowserRouter([
    {
        path: "/",
        element: <AuthLayout/>,
        children: [
            {
                path : "/login",
                element : <Login/>
            },
            {
                path : "/signup",
                element : <Signup/>
            }
        ]
    },
    {
        path : "/",
        element : <AdminLayout/>,
        children : [
            {
                path : "/admins",
                element : <AdminList/>
            },
            {
                path : "/admins/create",
                element : <AdminForm key={"adminCreate"}/>
            },
            {
                path : "/admins/update/:id",
                element : <AdminForm key={"adminUpdate"}/>
            },
        ]
    },
    {
        path : "/",
        element : <EmployeeLayout/>,
        children : [
            {
                path : "/employees",
                element : <EmployeeList/>
            },
            {
                path : "/employees/create",
                element : <EmployeeForm key={"employeeCreate"}/>
            },
            {
                path : "/employees/update/:id",
                element : <EmployeeForm key={"employeeUpdate"}/>
            },
        ]
    },
    {
        path : "/",
        element : <CompanyLayout/>,
        children : [
            {
                path : "/companies",
                element : <Companies/>
            },
            {
                path : "/companies/:id/departes/",
                element : <CompanyDepartes/>
            },
            {
                path : "/companies/:id/departes/:idDepartement",
                element : <CompanyDepartementInfos/>
            },

        ]
    },
    {
        path : "/",
        element : <DepartesLayout/>,
        children : [
            {
                path : "/departes",
                element : <DepartesList/>
            },
            {
                path : "/departes/create",
                element : <DepartesForm key={"departementCreate"}/>
            },
            {
                path : "/departes/update/:id",
                element : <DepartesForm key={"departementUpdate"}/>
            },

        ]
    },
    {
        path : "/",
        element : <JobLayout/>,
        children : [
            {
                path : "/jobs",
                element : <JobList/>
            },
            {
                path : "/jobs/create",
                element : <JobForm key={"jobCreate"}/>
            },
            {
                path : "/jobs/update/:id",
                element : <JobForm key={"jobUpdate"}/>
            },

        ]
    }
])

export default routes;
