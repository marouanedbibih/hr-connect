import React, { useEffect, useState } from "react";
import axiosClient from "../../api/axios";
import { useStateContext } from "../../context/ContextProvider";
import Header from "../../components/Header/Header";
import EmployeesTable from "../../components/Table/EmployeesTable";
import Paginations from "../../components/Pagination/Paginations";

function EmployeeList() {
  // Variables and Hooks
  const { setNotification } = useStateContext();
  const [employees, setEmployees] = useState([]);
  const [loading, setLoading] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);

  // Functions
  useEffect(() => {
    getEmployees(currentPage);
  }, [currentPage]);

  const getEmployees = (page) => {
    setLoading(true);
    axiosClient
      .get(`/employee?page=${page}`)
      .then(({ data }) => {
        setLoading(false);
        setEmployees(data.employees);
        setTotalPages(data.totalPages);
      })
      .catch(() => {
        setLoading(false);
      });
  };

  const onDeleteClick = (id) => {
    if (!window.confirm("Are you sure you want to delete this Employee !!")) {
      return;
    }
    axiosClient
      .delete(`/employee/${id}`)
      .then(() => {
        setNotification("Employee was successfully deleted");
        getEmployees(currentPage);
      })
      .catch((error) => {
        console.error(error);
      });
  };

  return (
    <div className="">
      {/* Employees Header */}
      <Header isButtonNew={true} title={"Employees"} />
      {/* Employees List */}
      <div className="grid grid-cols-1 justify-items-center mb-8">
        {/* Employees Table */}
        <EmployeesTable
          employees={employees}
          loading={loading}
          onDeleteClick={onDeleteClick}
        />
        {/* Employees Paginations */}
        <Paginations
          currentPage={currentPage}
          setCurrentPage={setCurrentPage}
          totalPages={totalPages}
        />
      </div>
    </div>
  );
}

export default EmployeeList;
