import React from 'react'
import { useNavigate, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import axiosClient from "../../api/axios.js";
import { useStateContext } from "../../context/ContextProvider.jsx";
import FormHeader from '../../components/Form/FormHeader';

function JobForm() {
  const navigate = useNavigate();
  let { id } = useParams();
  const [job, setJob] = useState({
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
        .get(`/job/${id}`)
        .then(({ data }) => {
          setLoading(false);
          setJob(data);
        })
        .catch(() => {
          setLoading(false);
        });
    }, []);
  }

  const onSubmit = (ev) => {
    ev.preventDefault();
    const payload = { ...job };
    if (id) {
      axiosClient
        .put(`/job/${id}`, payload)
        .then(() => {
          setNotification("Job was successfully updated");
          navigate("/jobs");
        })
        .catch((err) => {
          const response = err.response;
        });
    } else {
      axiosClient
        .post("/job", payload)
        .then((response) => {
          setNotification("Job was successfully created");
          navigate("/jobs");
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
      <FormHeader formName={"Job"} data={job} id={id}/>
      <div className="card animated fadeInDown">
        {loading && <div className="text-center">Loading...</div>}

        {!loading && (
          <form onSubmit={onSubmit} className="grid grid-cols-1 gap-4 ">
            <div className="grid grid-colms-1 gab-8 ">
              <input
                value={job.name}
                onChange={(ev) => setJob({ ...job, name: ev.target.value })}
                placeholder="Name of job"
                className="mb-4"
              />
              <input
                value={job.description}
                onChange={(ev) =>
                  setJob({ ...job, description: ev.target.value })
                }
                placeholder="Type description"
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

export default JobForm