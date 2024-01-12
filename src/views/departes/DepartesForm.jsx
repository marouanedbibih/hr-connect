import React from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import axiosClient from "../../api/axios.js";
import { useStateContext } from "../../context/ContextProvider.jsx";

function DepartesForm() {
  // Variables and Hooks
  const navigate = useNavigate();
  let { id } = useParams();
  const [departement, setDepartement] = useState({
    name: "",
    description: "",
  });
  const [errors, setErrors] = useState(null);
  const { setNotification } = useStateContext();
  const [loading, setLoading] = useState(false);

  // Functions
  if (id) {
    useEffect(() => {
      setLoading(true);
      axiosClient
        .get(`/departement/${id}`)
        .then(({ data }) => {
          setLoading(false);
          setDepartement(data);
        })
        .catch(() => {
          setLoading(false);
        });
    }, []);
  }
  
  const onSubmit = (ev) => {
    ev.preventDefault();
    const payload = { ...departement };
    if (id) {
      axiosClient
        .put(`/departement/${id}`, payload)
        .then(() => {
          setNotification("Departement was successfully updated");
          navigate("/departes");
        })
        .catch((err) => {
          const response = err.response;
        });
    } else {
      console.log("Payload:", payload);
      axiosClient
        .post("/departement", payload)
        .then((response) => {
          setNotification("Departement was successfully created");
          navigate("/departes");
        })
        .catch((err) => {
          setErrors(err.response.data.errors);
        });
    }
  };
  setTimeout(() => {
    setErrors(null)
  }, 3000)

  return (
    <>
      {errors && (
        <div className="alert">
          {errors.map((u) => (
            <p>{u}</p>
          ))}
        </div>
      )}
      <div className="w-full h-20  justify-between items-center inline-flex">
        <div className="text-black text-5xl font-bold font-['Roboto'] leading-[62.40px]">
          {id && <h1>Update Departement: {departement.name} </h1>}
          {!id && <h1>New Departement</h1>}
        </div>
      </div>
      <div className="card animated fadeInDown">
        {loading && <div className="text-center">Loading...</div>}

        {!loading && (
          <form onSubmit={onSubmit} className="grid grid-cols-1 gap-4 ">
            <div className="grid grid-colms-1 gab-8 ">
              <input
                value={departement.name}
                onChange={(ev) => setDepartement({ ...departement, name: ev.target.value })}
                placeholder="Name of departement"
                className="mb-4"
              />
              <input
                value={departement.description}
                onChange={(ev) =>
                  setDepartement({ ...departement, description: ev.target.value })
                }
                placeholder="text"
                className="mb-4"
              />
            </div>
            <div className="grid grid-cols-1 w-1/12">
              <button className="btn">Save</button>
            </div>
          </form>
        )}
      </div>
    </>
  );
}

export default DepartesForm