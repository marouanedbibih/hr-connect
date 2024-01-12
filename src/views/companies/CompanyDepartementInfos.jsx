import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { useStateContext } from "../../context/ContextProvider";
import axiosClient from "../../api/axios";
import { Link } from "react-router-dom";

import {
  TbPlayerTrackNextFilled,
  TbPlayerTrackPrevFilled,
} from "react-icons/tb";
import TableName from "../../components/Table/TableName";
import Paginations from "../../components/Pagination/Paginations";
import SelectForm from "../../components/Form/SelectForm";

function CompanyDepartementInfos() {
    let { id,idDepartement} = useParams();
    const [select, setSelect] = useState({
        value: null,
      })
    const [data, setData] = useState({
        companyId: id,
        departementId: idDepartement,
        jobId: null
      });
    const [departement, setDepartement] = useState("");
    const [company,setCompany] =useState("");
    const [jobs,setJobs] =useState([]);
    const [jobsSelect,setJobsSelect]=useState([]);
    const { setNotification } = useStateContext();
    const [loading, setLoading] = useState(false);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const carouselPages = 5;
  
    useEffect(() => {
      getJobsInfos(currentPage);
      if (id && idDepartement) {
        getJobsNotInDepartement()
      }
    }, [currentPage,id,idDepartement]);
  
    const getJobsNotInDepartement = () =>{
        setLoading(true);
        axiosClient
        .get(`/company-departement-job/jobs-not-in-company-department/${id}/${idDepartement}`)
        .then(({ data }) => {
          setLoading(false);
          setJobsSelect(data);
        })
        .catch(() => {
          setLoading(false);
        });
    }
    const getJobsInfos = (page) => {
      setLoading(true);
      axiosClient
        .get(`/company-departement-job/${id}/${idDepartement}?page=${page}`)
        .then(({ data }) => {
            console.log("CDJ",data)
            setCompany(data.companyName);
            setDepartement(data.departmentName);
            setJobs(data.jobs);
            setTotalPages(data.totalPages);
            setLoading(false);
        })
        .catch(() => {
          setLoading(false);
        });
    };

    const onSubmit = (ev) => {
        ev.preventDefault();
        console.log("Value of selected" ,select.value);
        data.jobId = select.value
        const payload = { ...data }; // Fix: Change from admin to company
        if (id,idDepartement) {
          axiosClient
            .post(`/company-departement-job`, payload) // Fix: Change from /admin to /company
            .then(() => {
              setNotification("Job was successfully added to Departement of Company");
              getJobsInfos(1);
              getJobsNotInDepartement();
              setSelect({ value: null });
            })
            .catch((err) => {
              const response = err.response;
              console.log("Error Company Update", err);
            });
        }
      };

    const onRemoveClick = (idJob) =>{
        if (!window.confirm("Are you sure you want to Remove this job from this Department of Company  !!")) {
            return;
          }
          axiosClient
            .delete(`/company-departement-job/${id}/${idDepartement}/jobs/${idJob}`)
            .then(() => {
              setNotification("Job was successfully removed from this Departement of Company");
              getJobsInfos(1);
              getJobsNotInDepartement();
              setSelect({ value: null });
            })
            .catch((error) => {
              console.error(error);
            });
    }
  return (
    <div>
    {/* Customers Header */}
      <div className="w-full h-20  justify-between items-center inline-flex">
        <div className="text-black text-5xl font-bold font-['Roboto'] leading-[62.40px]">
          {`${company} : ${departement}`}
        </div>
        <div className="flex justify-center items-center">
          <Link
            to="add"
            className="w-[81px] px-3.5 py-2 bg-emerald-600 rounded-lg shadow justify-center items-center gap-2 flex"
          >
            <div className="text-white text-xs font-bold font-['Roboto'] uppercase leading-[18px]">
              add
            </div>
          </Link>
        </div>
      </div>
      <div className="flex justify-start items-start gap-8 ">
        <div
          className="grid grid-cols-1 justify-items-center mb-8"
          id="company-departement-jobs-list"
        >
          {/* Company Departement Jobs Table */}
          <TableName loading={loading} datas={jobs} onRemoveClick={onRemoveClick} url={"company-departement-job"} />
          {/* Company Departement Jobs Paginations */}
          <Paginations currentPage={currentPage} setCurrentPage={setCurrentPage} totalPages={totalPages}/>
          
        </div>
        {/* <CompanyDepartesForm/> */}
        <SelectForm datas={jobsSelect} loading={loading} playload={data} select={select} setSelect={setSelect} onSubmit={onSubmit} />

      </div>
    </div>
  )
}

export default CompanyDepartementInfos