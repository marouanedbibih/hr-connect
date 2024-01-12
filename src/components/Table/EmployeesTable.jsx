import React from "react";
import { Link } from "react-router-dom";

function EmployeesTable({loading,employees,onDeleteClick}) {
  return (
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
              Company
            </th>
            <th className="px-4 py-2 text-left lg:text-sm bg-gray-300">
              Departement
            </th>
            <th className="px-4 py-2 text-left lg:text-sm bg-gray-300">
              Job
            </th>
            <th className="px-4 py-2 text-left lg:text-sm bg-gray-300">
              Level
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
              employees.map((e) => (
                <tr key={e.id} className="">
                  <td className="px-4 py-2">
                    <div className=" flex items-center justify-start">
                      <div className="whitespace-normal">
                        <p className="lg:text-sm">{e.name}</p>
                      </div>
                    </div>
                  </td>
                  <td className="">
                    <p className="lg:text-sm">{e.email}</p>
                  </td>
                  <td className="">
                    <p className="lg:text-sm">{e.phone}</p>
                  </td>
                  <td className="">
                    <p className="lg:text-sm">{e.companyName}</p>
                  </td>
                  <td className="">
                    <p className="lg:text-sm">{e.departementName}</p>
                  </td>
                  <td className="">
                    <p className="lg:text-sm">{e.jobName}</p>
                  </td>
                  <td className="">
                    <p className="lg:text-sm">{e.level}</p>
                  </td>

                  <td className="flex items-center">
                    <Link
                      to={"/employees/update/" + e.id}
                      className="w-auto px-3.5 py-2 mr-2 bg-emerald-600 rounded-lg shadow justify-center items-center gap-2 flex"
                    >
                      <div className="text-white text-xs font-bold font-['Roboto'] uppercase leading-[18px]">
                        Edit
                      </div>
                    </Link>

                    <button
                      onClick={(ev) => onDeleteClick(e.id)}
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
  );
}

export default EmployeesTable;
