import Header from "../../components/Header/Header";
import TableNameDescription from "../../components/Table/TableNameDescription";
import React, { useEffect, useState } from "react";
import axiosClient from "../../api/axios";
import { useStateContext } from "../../context/ContextProvider";
import Paginations from "../../components/Pagination/Paginations";

function JobList() {
  // Variables and Hooks
  const { setNotification } = useStateContext();
  const [jobs, setJobs] = useState([]);
  const [loading, setLoading] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);

  // Functions
  useEffect(() => {
    getJobs(currentPage);
  }, [currentPage]);

  const getJobs = (page) => {
    setLoading(true);
    axiosClient
      .get(`/job?page=${page}`)
      .then(({ data }) => {
        console.log("Jobs Datas", data.jobs);
        setLoading(false);
        setJobs(data.jobs);
        setTotalPages(data.totalPages);
      })
      .catch(() => {
        setLoading(false);
      });
  };

  const onDeleteClick = (id) => {
    if (!window.confirm("Are you sure you want to delete this Job !!")) {
      return;
    }
    axiosClient
      .delete(`/job/${id}`)
      .then(() => {
        setNotification("Job was successfully deleted");
        getJobs(currentPage);
      })
      .catch((error) => {
        console.error(error);
      });
  };

  return (
    <div className="">
      {/* Jobs Header */}
      <Header title={"Jobs"} isButtonNew={true} />
      {/* Jobs List */}
      <div className="grid grid-cols-1 justify-items-center mb-8">
        <TableNameDescription
          loading={loading}
          datas={jobs}
          url={"jobs"}
          onDeleteClick={onDeleteClick}
        />
        <Paginations
          currentPage={currentPage}
          setCurrentPage={setCurrentPage}
          totalPages={totalPages}
        />
      </div>
    </div>
  );
}

export default JobList;
