import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import axiosClient from "../../api/axios";
import {
  TbPlayerTrackNextFilled,
  TbPlayerTrackPrevFilled,
} from "react-icons/tb";
import { useStateContext } from "../../context/ContextProvider";

function AdminList() {
  const { setNotification } = useStateContext();
  const [admins, setAdmins] = useState([]);
  const [loading, setLoading] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const carouselPages = 5;

  useEffect(() => {
    getAdmins(currentPage);
  }, [currentPage]);

  const getAdmins = (page) => {
    setLoading(true);
    axiosClient
      .get(`/admin?page=${page}`)
      .then(({ data }) => {
        console.log("Admins Datas", data);
        setLoading(false);
        setAdmins(data.admins);
        setTotalPages(data.totalPages);
      })
      .catch(() => {
        setLoading(false);
      });
  };


  const onDeleteClick = (id) => {
    if (!window.confirm("Are you sure you want to delete this Admin !!")) {
      return;
    }
    axiosClient
      .delete(`/admin/${id}`)
      .then(() => {
        setNotification("Admin was successfully deleted");
        getAdmins(currentPage);
      })
      .catch((error) => {
        console.error(error);
      });
  };

  const onChangePage = (newPage) => {
    if (newPage >= 1 && newPage <= totalPages) {
      setCurrentPage(newPage);
    }
  };

  const onPreviousPage = () => {
    if (currentPage > 1) {
      onChangePage(currentPage - 1);
    }
  };

  const onNextPage = () => {
    if (currentPage < totalPages) {
      onChangePage(currentPage + 1);
    }
  };

  return (
    <div className="">
      {/* Customers Header */}
      <div className="w-full h-20  justify-between items-center inline-flex">
        <div className="text-black text-5xl font-bold font-['Roboto'] leading-[62.40px]">
          Admins
        </div>
        <div className="flex justify-center items-center">


          <Link
            to="create"
            className="w-[81px] px-3.5 py-2 bg-emerald-600 rounded-lg shadow justify-center items-center gap-2 flex"
          >
            <div className="text-white text-xs font-bold font-['Roboto'] uppercase leading-[18px]">
              Add new
            </div>
          </Link>
        </div>
      </div>

      {/* Customers List */}
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
                  Email
                </th>
                <th className="px-4 py-2 text-left lg:text-sm bg-gray-300">
                  Phone
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
                  admins.map((a) => (
                    <tr key={a.adminId} className="">
                      <td className="px-4 py-2">
                        <div className=" flex items-center justify-start">
                          <div className="whitespace-normal">
                            <p className="lg:text-sm">{a.user.name}</p>
                          </div>
                        </div>
                      </td>
                      <td className="">
                        <p className="lg:text-sm">{a.user.email}</p>
                      </td>

                      <td className="">
                        <p className="lg:text-sm">{a.user.phone}</p>
                      </td>

                      <td className="flex items-center">
                        <Link
                          to={"/admins/update/" + a.adminId}
                          className="w-auto px-3.5 py-2 mr-2 bg-emerald-600 rounded-lg shadow justify-center items-center gap-2 flex"
                        >
                          <div className="text-white text-xs font-bold font-['Roboto'] uppercase leading-[18px]">
                            Edit
                          </div>
                        </Link>

                        <button
                          onClick={(ev) => onDeleteClick(a.adminId)}
                          className="w-auto px-3.5 py-2 bg-red-500 rounded-lg shadow justify-center items-center gap-2 flex"
                        >
                          <div className="text-white text-xs font-bold font-['Roboto'] uppercase leading-[18px]">
                            Delete
                          </div>
                        </button>
                      </td>
                    </tr>
                  ))}
              </tbody>
            )}
          </table>
        </div>
        {/* Customers Paginations */}
        <div className="w-64 h-10 flex justify-center items-center space-x-4">
          <button
            className={`w-[90px] h-10 px-4 py-2.5  rounded-lg justify-center items-center gap-2 inline-flex text-white text-xs font-bold font-['Roboto'] uppercase leading-[18px]  ${
              currentPage === 1 ? "bg-violet-800" : "bg-emerald-600"
            }`}
            onClick={onPreviousPage}
          >
            <TbPlayerTrackPrevFilled />
          </button>

          {Array.from({ length: totalPages }, (_, index) => {
            // Calculate the start and end pages for the carousel
            let startPage = currentPage - Math.floor(carouselPages / 2);
            let endPage = startPage + carouselPages - 1;

            // Ensure pages stay within bounds
            if (startPage < 1) {
              startPage = 1;
              endPage = startPage + carouselPages - 1;
            }
            if (endPage > totalPages) {
              endPage = totalPages;
              startPage = endPage - carouselPages + 1;
            }

            // Display buttons within the carousel range
            if (index + 1 >= startPage && index + 1 <= endPage) {
              return (
                <button
                  key={index}
                  className={`w-[39px] h-10 px-4 py-2.5  rounded-lg justify-center items-center gap-2 inline-flex text-white text-xs font-bold font-['Roboto'] uppercase leading-[18px]  ${
                    currentPage === index + 1
                      ? "bg-emerald-600"
                      : "bg-violet-800"
                  }`}
                  onClick={() => onChangePage(index + 1)}
                >
                  {index + 1}
                </button>
              );
            }
            return null; // Return null for buttons outside the carousel range
          })}
          <button
            className={`w-[90px] h-10 px-4 py-2.5  rounded-lg justify-center items-center gap-2 inline-flex text-white text-xs font-bold font-['Roboto'] uppercase leading-[18px]  ${
              currentPage === totalPages ? "bg-violet-800" : "bg-emerald-600"
            }`}
            onClick={onNextPage}
          >
            <TbPlayerTrackNextFilled />
          </button>
        </div>
      </div>
    </div>
  );
}

export default AdminList;
