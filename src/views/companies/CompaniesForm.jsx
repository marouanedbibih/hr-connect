import { useNavigate, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import axiosClient from "../../api/axios.js";
import { useStateContext } from "../../context/ContextProvider.jsx";

function CompaniesForm({ selectedCompanyId, onCancelEdit,getCompanies }) {
  const navigate = useNavigate();
  const [company, setCompany] = useState({
    name: "",
    size: "Small",
  });
  const defaultCompany = {
    name: "",
    size: "Small",
  };
  const [errors, setErrors] = useState(null);
  const { setNotification } = useStateContext();
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (selectedCompanyId) {
      setLoading(true);
      console.log("Id Company exist", selectedCompanyId);
      axiosClient
        .get(`/company/${selectedCompanyId}`)
        .then(({ data }) => {
          setLoading(false);
          console.log("Company", data);
          setCompany(data);
        })
        .catch(() => {
          setLoading(false);
        });
    }
  }, [selectedCompanyId]);
  const onHandleCancelEdit = () =>{
    onCancelEdit();
    setCompany(defaultCompany);
  }

  const onSubmit = (ev) => {
    ev.preventDefault();
    const payload = { ...company }; // Fix: Change from admin to company
    if (selectedCompanyId) {
      axiosClient
        .put(`/company/${selectedCompanyId}`, payload) // Fix: Change from /admin to /company
        .then(() => {
          setNotification("Company was successfully updated");
          setCompany(defaultCompany);
          getCompanies(1)
          onHandleCancelEdit()
        })
        .catch((err) => {
          const response = err.response;
          console.log("Error Company Update", err);
        });
    } else {
      axiosClient
        .post("/company", payload) // Fix: Change from /admin to /company
        .then((response) => {
          setNotification("Company was successfully created");
          setCompany(defaultCompany);
          getCompanies(1)
        })
        .catch((err) => {
        //   setErrors(err.response.data.errors);
        console.log("Error Company Create", err);

        });
    }
  };

  setTimeout(() => {
    setErrors(null);
  }, 3000);

  return (
    <>
      {/* {errors && (
        <div className="alert">
          {errors.map((u, index) => (
            <p key={index}>{u}</p>
          ))}
        </div>
      )} */}
      <div className="w-full card animated fadeInDown">
        {loading && <div className="text-center">Loading...</div>}

        {!loading && (
          <form onSubmit={onSubmit} className="grid grid-cols-1 gap-4">
            <div className="grid grid-cols-1 gap-8">
              <input
                value={company.name}
                onChange={(ev) =>
                  setCompany({ ...company, name: ev.target.value })
                }
                placeholder="Name of company"
                className="mb-4"
              />
              {/* Replace the input with a dropdown select */}
              <select
                value={company.size}
                onChange={(ev) =>
                  setCompany({ ...company, size: ev.target.value })
                }
                className="mb-4"
              >
                <option value="Small">Small</option>
                <option value="Medium">Medium</option>
                <option value="Large">Large</option>
              </select>
            </div>
            <div className="grid grid-cols-2 gap-8 w-1/2 items-center">
              <button type="submit" className="w-auto px-3.5 py-2 bg-violet-800 rounded-lg shadow justify-center items-center gap-2 flex text-white text-xs font-bold font-['Roboto'] uppercase leading-[18px]">
                Save
              </button>
              {selectedCompanyId && (
                <button
                  type="button"
                  onClick={onHandleCancelEdit}
                  className=" w-auto px-3.5 py-2 bg-red-500 rounded-lg shadow justify-center items-center gap-2 flex text-white text-xs font-bold font-['Roboto'] uppercase leading-[18px]"
                >
                  Cancel
                </button>
              )}
            </div>
          </form>
        )}
      </div>
    </>
  );
}

export default CompaniesForm;
