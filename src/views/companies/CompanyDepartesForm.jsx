import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import axiosClient from '../../api/axios.js';
import { useStateContext } from '../../context/ContextProvider.jsx';

function CompanyDepartesForm({getCompanyDepartes}) {
  const navigate = useNavigate();
  const { id } = useParams();
  const [departes, setDepartes] = useState([]);
  const [CompanyDepartement, setCompanyDepartement] = useState({
    departementId: null,
    companyId: id
  });
  const [errors, setErrors] = useState(null);
  const { setNotification } = useStateContext();
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (id) {
      setLoading(true);
      axiosClient
        .get(`/company-departes/company/${id}/departments/not-in-company`)
        .then(({ data }) => {
          setLoading(false);
          setDepartes(data);
        })
        .catch(() => {
          setLoading(false);
        });
    }
  }, [id]);

  const onSubmit = (ev) => {
    ev.preventDefault();
    const payload = { ...CompanyDepartement }; // Fix: Change from admin to company
    if (id) {
      axiosClient
        .post(`company-departes/add-department-to-company`, payload) // Fix: Change from /admin to /company
        .then(() => {
          setNotification("Departement was successfully added");
          getCompanyDepartes(1);
        })
        .catch((err) => {
          const response = err.response;
          console.log("Error Company Update", err);
        });
    }
  };

  setTimeout(() => {
    setErrors(null);
  }, 3000);

  return (
    <>
      <div className="w-1/2 card animated fadeInDown">
        {loading && <div className="text-center">Loading...</div>}

        {!loading && (
          <form onSubmit={onSubmit} className="grid grid-cols-1 gap-4">
            <div className="grid grid-cols-1 gap-8">
              <select
                value={CompanyDepartement.departementId}
                onChange={(ev) =>
                  setCompanyDepartement({ ...CompanyDepartement, departementId: ev.target.value })
                }
                className="mb-4"
              >
                {departes.map((d) => (
                  <option key={d.id} value={d.id}>
                    {d.name}
                  </option>
                ))}
              </select>
            </div>
            <div className="grid grid-cols-2 gap-8 w-1/2 items-center">
              <button
                type="submit"
                className="w-auto px-3.5 py-2 bg-violet-800 rounded-lg shadow justify-center items-center gap-2 flex text-white text-xs font-bold font-['Roboto'] uppercase leading-[18px]"
              >
                Save
              </button>
            </div>
          </form>
        )}
      </div>
    </>
  );
}

export default CompanyDepartesForm;
