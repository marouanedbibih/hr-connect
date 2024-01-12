import React from "react";
import { Link } from "react-router-dom";

function CompaniesList({ company, onDeleteClick, onEditClick }) {
  return (
    <tr key={company.id} className="">
      <td className="px-4 py-2">
        <div className="flex items-center justify-start">
          <div className="whitespace-normal">
            <p className="lg:text-sm">{company.name}</p>
          </div>
        </div>
      </td>
      <td className="">
        <p className="lg:text-sm">{company.size}</p>
      </td>
      <td className="flex items-center">
        <Link
          to={`/companies/${company.id}/departes`}
          className="w-auto px-3.5 py-2 mr-2 bg-violet-800 rounded-lg shadow justify-center items-center gap-2 flex"
        >
          <div className="text-white text-xs font-bold font-['Roboto'] uppercase leading-[18px]">
            view
          </div>
        </Link>
        <Link
          onClick={(ev) => onEditClick(company.id)}
          className="w-auto px-3.5 py-2 mr-2 bg-emerald-600 rounded-lg shadow justify-center items-center gap-2 flex"
        >
          <div className="text-white text-xs font-bold font-['Roboto'] uppercase leading-[18px]">
            Edit
          </div>
        </Link>
        <button
          onClick={(ev) => onDeleteClick(company.id)}
          className="w-auto px-3.5 py-2 bg-red-500 rounded-lg shadow justify-center items-center gap-2 flex"
        >
          <div className="text-white text-xs font-bold font-['Roboto'] uppercase leading-[18px]">
            Delete
          </div>
        </button>
      </td>
    </tr>
  );
}

export default CompaniesList;
