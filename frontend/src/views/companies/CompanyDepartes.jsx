import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { useStateContext } from "../../context/ContextProvider";
import axiosClient from "../../api/axios";
import { Link } from "react-router-dom";

import {
  TbPlayerTrackNextFilled,
  TbPlayerTrackPrevFilled,
} from "react-icons/tb";
import CompanyDepartesForm from "./CompanyDepartesForm";
import Paginations from "../../components/Pagination/Paginations";

function CompanyDepartes() {
  let { id} = useParams();
  const [company, setCompany] = useState({
    id: "",
    name: "",
    size: "",
  });
  const [departes, setDepartes] = useState([]);
  const { setNotification } = useStateContext();
  const [loading, setLoading] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const carouselPages = 5;

  useEffect(() => {
    getCompanyDepartes(currentPage);
  }, [currentPage]);

  const getCompanyDepartes = (page) => {
    setLoading(true);
    axiosClient
      .get(`/company-departes/company/${id}/departments?page=${page}`)
      .then(({ data }) => {
        setCompany({
          id: data.id,
          name: data.name,
          size: data.size,
        });
        setDepartes(data.departments);
        setTotalPages(data.totalPages);
        setLoading(false);
      })
      .catch(() => {
        setLoading(false);
      });
  };

  const onDeleteClick = (companyId,departementId) => {
    if (!window.confirm("Are you sure you want to remove this Departement from this company  !!")) {
      return;
    }
    axiosClient
      .delete(`/company-departes/${companyId}/${departementId}`)
      .then(() => {
        setNotification("Departement was successfully removed from the Company");
        getCompanyDepartes(currentPage);
      })
      .catch((error) => {
        console.error(error);
      });
  };


  return (
    <div className="">
      {/* Customers Header */}
      <div className="w-full h-20  justify-between items-center inline-flex">
        <div className="text-black text-5xl font-bold font-['Roboto'] leading-[62.40px]">
          {`${company.name} : ${company.size}`}
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
          id="user-list"
        >
          {/* Customers Table */}
          <div className="bg-white rounded-md shadow-md p-5 mb-4 mt-2 animated fadeInDown">
            <table className="w-full">
              <thead className="bg-gray-300">
                <tr>
                  <th className="px-4 py-2 flex items-center text-left lg:text-sm bg-gray-300">
                    Name
                  </th>
                  <th className="px-4 py-2 text-left lg:text-sm bg-gray-300">
                    Actions
                  </th>
                </tr>
              </thead>
              {loading && (
                <tbody>
                  <tr>
                    <td colSpan="5" className="text-center">
                      Loading...
                    </td>
                  </tr>
                </tbody>
              )}
              {!loading && (
                <tbody>
                  {!loading &&
                    departes.map((d) => (
                      <tr key={d.id} className="">
                        <td className="px-4 py-2">
                          <div className=" flex items-center justify-start">
                            <div className="whitespace-normal">
                              <p className="lg:text-sm">{d.name}</p>
                            </div>
                          </div>
                        </td>
                        <td className="flex items-center">
                          <Link
                            to={`/companies/${id}/departes/${d.id}`}
                            className="w-auto px-3.5 py-2 mr-2 bg-violet-800 rounded-lg shadow justify-center items-center gap-2 flex"
                          >
                            <div className="text-white text-xs font-bold font-['Roboto'] uppercase leading-[18px]">
                              View
                            </div>
                          </Link>

                          <button
                            onClick={(ev) => onDeleteClick(id,d.id)}
                            className="w-auto px-3.5 py-2 bg-red-500 rounded-lg shadow justify-center items-center gap-2 flex"
                          >
                            <div className="text-white text-xs font-bold font-['Roboto'] uppercase leading-[18px]">
                              Remove
                            </div>
                          </button>
                        </td>
                      </tr>
                    ))}
                </tbody>
              )}
            </table>
          </div>
          {/* Departes of Company Paginations */}
          <Paginations currentPage={currentPage} setCurrentPage={setCurrentPage} totalPages={totalPages}/>
        </div>
        <CompanyDepartesForm getCompanyDepartes={getCompanyDepartes}/>

      </div>

      {/* Customers List */}
    </div>
  );
}

export default CompanyDepartes;
