import { useNavigate, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import axiosClient from "../../api/axios.js";
import { useStateContext } from "../../context/ContextProvider.jsx";

function EmployeeForm() {
  // Variables and Hooks 
  const navigate = useNavigate();
  let { id } = useParams();
  const [employee, setEmployee] = useState({
    name: "",
    email: "",
    phone: "",
    password: "",
    confirm: "",
    companyId: "",
    departementId: "",
    jobId: "",
    level: "",
  });

  const [companies, setCompanies] = useState([]);
  const [departes, setDepartes] = useState([]);
  const [jobs, setJobs] = useState([]);

  const [companyId, setCompanyId] = useState("");
  const [departementId, setDepartementId] = useState("");
  
  const [errors, setErrors] = useState(null);
  const { setNotification } = useStateContext();
  const [loading, setLoading] = useState(false);

  // Functions
  useEffect(() => {
    getCompanies();
  }, []);

  const getCompanies = () => {
    axiosClient
      .get(`/company/all`)
      .then(({ data }) => {
        setLoading(false);
        setCompanies(data.companies);
      })
      .catch(() => {
        setLoading(false);
      });
  };

  const getDepartes = (companyId) => {
    axiosClient
      .get(`/company-departes/company/${companyId}/all-departes`)
      .then(({ data }) => {
        setDepartes(data.departments);
      })
      .catch(() => {
        setLoading(false);
      });
  };

  const getJobs = (companyId, departementId) => {
    axiosClient
      .get(`/company-departement-job/all/${companyId}/${departementId}`)
      .then(({ data }) => {
        setJobs(data.jobs);
      })
      .catch(() => {
        setLoading(false);
      });
  };

  if (id) {
    useEffect(() => {
      setLoading(true);
      axiosClient
        .get(`/employee/${id}`)
        .then(({ data }) => {
          setLoading(false);
          setEmployee(data);
          setCompanyId(data.companyId)
          getDepartes(data.companyId)
          setDepartementId(data.departementId)
          getJobs(data.companyId,data.departementId)
        })
        .catch(() => {
          setLoading(false);
        });
    }, []);
  }

  const onSubmit = (ev) => {
    ev.preventDefault();
    const payload = { ...employee };
    if (id) {
      axiosClient
        .put(`/employee/${id}`, payload)
        .then(() => {
          setNotification("Employee was successfully updated");
          navigate("/employees");
        })
        .catch((err) => {
          const response = err.response;
        });
    } else {
      console.log("Payload:", payload);
      axiosClient
        .post("/employee", payload)
        .then((response) => {
          setNotification("Employee was successfully created");
          navigate("/employees");
        })
        .catch((err) => {
          setErrors(err.response.data.errors);
        });
    }
  };
  setTimeout(() => {
    setErrors(null);
  }, 3000);

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
          {id && <h1>Update Employee: {employee.name} </h1>}
          {!id && <h1>New employee</h1>}
        </div>
      </div>
      <div className="card animated fadeInDown">
        {loading && <div className="text-center">Loading...</div>}

        {!loading && (
          <form onSubmit={onSubmit} className="grid grid-cols-1 gap-4 ">
            <div className="grid grid-colms-1 gab-8 ">
              <input
                value={employee.name}
                onChange={(ev) =>
                  setEmployee({ ...employee, name: ev.target.value })
                }
                placeholder="Full Name"
                className="mb-4"
              />
              <input
                value={employee.email}
                onChange={(ev) =>
                  setEmployee({ ...employee, email: ev.target.value })
                }
                placeholder="Email"
                className="mb-4"
              />
              <input
                value={employee.phone}
                onChange={(ev) =>
                  setEmployee({ ...employee, phone: ev.target.value })
                }
                placeholder="Phone"
                className="mb-4"
              />

              <input
                type="password"
                onChange={(ev) =>
                  setEmployee({ ...employee, password: ev.target.value })
                }
                placeholder="Password"
                className="mb-4"
              />
              <input
                type="password"
                onChange={(ev) =>
                  setEmployee({ ...employee, confirm: ev.target.value })
                }
                placeholder="Password Confirmation"
                className="mb-4"
              />
              <select
                value={employee.companyId}
                onChange={(ev) => {
                  const selectedCompanyId = ev.target.value;
                  setEmployee({ ...employee, companyId: selectedCompanyId });
                  setCompanyId(selectedCompanyId);
                  getDepartes(selectedCompanyId); 
                  getJobs(selectedCompanyId,departementId)
                }}
                className="mb-4"
              >
                <option value="" disabled>
                  Choose Company
                </option>
                {companies.map((c) => (
                  <option key={c.id} value={c.id}>
                    {c.name}
                  </option>
                ))}
              </select>

              <select
                value={employee.departementId}
                onChange={(ev) => {
                  const selectedDepartementId = ev.target.value;
                  setEmployee({ ...employee, departementId: ev.target.value });
                  setDepartementId(selectedDepartementId);
                  getJobs(companyId, selectedDepartementId);
                }}
                className="mb-4"
              >
                <option value="" disabled>
                  Choose Department
                </option>
                {departes.length > 0 &&
                  departes.map((d) => (
                    <option key={d.id} value={d.id}>
                      {d.name}
                    </option>
                  ))}
                {departes.length == 0 && !loading && <option>No depts</option>}
                {departes.length == 0 && loading && <option>...loading</option>}
              </select>
              <select
                value={employee.jobId}
                onChange={(ev) =>
                  setEmployee({ ...employee, jobId: ev.target.value })
                }
                className="mb-4"
              >
                <option value="" disabled>
                  Choose Jobs
                </option>
                {jobs.map((j) => (
                  <option key={j.id} value={j.id}>
                    {j.name}
                  </option>
                ))}
              </select>
              <select
                value={employee.level}
                onChange={(ev) =>
                  setEmployee({ ...employee, level: ev.target.value })
                }
                className="mb-4"
              >
                <option value="" disabled>
                  Choose Employee Level
                </option>
                <option value="Intern">Intern</option>
                <option value="Junior">Junior</option>
                <option value="MidLevel">MidLevel</option>
                <option value="Senior">Senior</option>
                <option value="TeamLead">Team Lead</option>
                <option value="Cto">CTO</option>
                <option value="Architect">Architect</option>
              </select>
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

export default EmployeeForm;
