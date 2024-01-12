import React, { useEffect, useState } from "react";
import axiosClient from "../../api/axios";
import { useStateContext } from "../../context/ContextProvider";
import Header from "../../components/Header/Header";
import Paginations from "../../components/Pagination/Paginations";
import TableNameDescription from "../../components/Table/TableNameDescription";

function DepartesList() {
  // Variables and Hooks
  const { setNotification } = useStateContext();
  const [departes, setDepartes] = useState([]);
  const [loading, setLoading] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);

  useEffect(() => {
    getDepartes(currentPage);
  }, [currentPage]);

  // Functions
  const getDepartes = (page) => {
    setLoading(true);
    axiosClient
      .get(`/departement?page=${page}`)
      .then(({ data }) => {
        console.log("Departes Datas", data);
        setLoading(false);
        setDepartes(data.departements);
        setTotalPages(data.totalPages);
      })
      .catch(() => {
        setLoading(false);
      });
  };

  const onDeleteClick = (id) => {
    if (
      !window.confirm("Are you sure you want to delete this Departement !!")
    ) {
      return;
    }
    axiosClient
      .delete(`/departement/${id}`)
      .then(() => {
        setNotification("Departement was successfully deleted");
        getDepartes(currentPage);
      })
      .catch((error) => {
        console.error(error);
      });
  };

  return (
    <div className="">
      {/* Departess Header */}
      <Header isButtonNew={true} title={"Departes"} />
      {/* Departes List */}
      <div className="grid grid-cols-1 justify-items-center mb-8">
        {/* Departes Table */}
        <TableNameDescription
          datas={departes}
          loading={loading}
          url={"departes"}
          onDeleteClick={onDeleteClick}
        />
        {/* Departes Paginations */}
        <Paginations
          currentPage={currentPage}
          setCurrentPage={setCurrentPage}
          totalPages={totalPages}
        />
      </div>
    </div>
  );
}

export default DepartesList;
