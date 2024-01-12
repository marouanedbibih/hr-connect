import React, { useEffect, useState } from "react";
import axiosClient from "../../api/axios";
import { useStateContext } from "../../context/ContextProvider";
import CompaniesList from "./CompaniesList";
import CompaniesForm from "./CompaniesForm";
import Paginations from "../../components/Pagination/Paginations";
import Header from "../../components/Header/Header";

function Companies() {
  // Variables and Hooks
  const { setNotification } = useStateContext();
  const [companies, setCompanies] = useState([]);
  const [loading, setLoading] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [selectedCompanyId, setSelectedCompanyId] = useState(null);
  const [editing, setEditing] = useState(false);

  useEffect(() => {
    getCompanies(currentPage);
  }, [currentPage]);

  // Functions
  // Api functions
  const getCompanies = (page) => {
    setLoading(true);
    axiosClient
      .get(`/company?page=${page}`)
      .then(({ data }) => {
        setLoading(false);
        setCompanies(data.companies);
        setTotalPages(data.totalPages);
      })
      .catch(() => {
        setLoading(false);
      });
  };

  const onDeleteClick = (id) => {
    if (!window.confirm("Are you sure you want to delete this Company?")) {
      return;
    }
    axiosClient
      .delete(`/company/${id}`)
      .then(() => {
        setNotification("Company was successfully deleted");
        getCompanies(currentPage);
      })
      .catch((error) => {
        console.error(error);
      });
  };

  // Frontend functions
  const onEditClick = (id) => {
    setSelectedCompanyId(id);
    setEditing(true);
  };

  const onCancelEdit = () => {
    setSelectedCompanyId(null);
    setEditing(false);
  };



  return (
    <div className="">
      {/* Companies Header */}
      <Header isButtonNew={false} title={"Companies"} />
      <div className="w-full justify-start items-start inline-flex gap-4">
        {/* Companies List */}
        <div className="w-1/2 grid grid-cols-1 justify-items-center mb-8">
          {/* Companiess Table */}
          <div className="w-full bg-white rounded-md shadow-md p-5 mb-4 mt-2 animated fadeInDown">
            <table className="w-full">
              <thead className="bg-gray-300">
                <tr>
                  <th className="px-4 py-2 flex items-center text-left lg:text-sm bg-gray-300">
                    Name
                  </th>
                  <th className="px-4 py-2 text-left lg:text-sm bg-gray-300">
                    Size
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
                    companies.map((company) => (
                      <CompaniesList
                        key={company.id}
                        company={company}
                        onDeleteClick={onDeleteClick}
                        onEditClick={onEditClick}
                      />
                    ))}
                </tbody>
              )}
            </table>
          </div>
          <Paginations
            setCurrentPage={setCurrentPage}
            currentPage={currentPage}
            totalPages={totalPages}
          />
        </div>

        {/* Companies Form */}
        <div className="w-1/2 grid grid-cols-1 justify-items-center mb-8">
          <CompaniesForm
            selectedCompanyId={selectedCompanyId}
            onCancelEdit={onCancelEdit}
            getCompanies={getCompanies}
          />
        </div>
      </div>
    </div>
  );
}

export default Companies;
