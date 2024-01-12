import { useNavigate, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import axiosClient from "../../api/axios.js";
import { useStateContext } from "../../context/ContextProvider.jsx";

function AdminForm() {
  const navigate = useNavigate();
  let { id } = useParams();
  console.log("Admin Update ID",id)
  const [admin, setAdmin] = useState({
    name: "",
    email: "",
    phone:"",
    password: "",
    confirm: "",
  });
  const [errors, setErrors] = useState(null);
  const { setNotification } = useStateContext();
  const [loading, setLoading] = useState(false);

  if (id) {
    useEffect(() => {
      setLoading(true);
      axiosClient
        .get(`/admin/${id}`)
        .then(({ data }) => {
          setLoading(false);
          setAdmin(data.user);
          // console.log("Admin Data Update",data.user)
        })
        .catch(() => {
          setLoading(false);
        });
    }, []);
  }

  const onSubmit = (ev) => {
    ev.preventDefault();
    const payload = { ...admin };
    if (id) {
      axiosClient
        .put(`/admin/${id}`, payload)
        .then(() => {
          setNotification("Admin was successfully updated");
          navigate("/admins");
        })
        .catch((err) => {
          const response = err.response;
          console.log("Error Admin",err);

        });
    } else {
      console.log("Payload:", payload);
      axiosClient
        .post("/admin", payload)
        .then((response) => {
          setNotification("Admin was successfully created");
          navigate("/admins");
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
          {id && <h1>Update Admin: {admin.name} </h1>}
          {!id && <h1>New Admin</h1>}
        </div>
      </div>
      <div className="card animated fadeInDown">
        {loading && <div className="text-center">Loading...</div>}

        {!loading && (
          <form onSubmit={onSubmit} className="grid grid-cols-1 gap-4 ">
            <div className="grid grid-colms-1 gab-8 ">
              <input
                value={admin.name}
                onChange={(ev) => setAdmin({ ...admin, name: ev.target.value })}
                placeholder="Full Name"
                className="mb-4"
              />
              <input
                value={admin.email}
                onChange={(ev) =>
                  setAdmin({ ...admin, email: ev.target.value })
                }
                placeholder="Email"
                className="mb-4"
              />
              <input
                value={admin.phone}
                onChange={(ev) =>
                  setAdmin({ ...admin, phone: ev.target.value })
                }
                placeholder="Phone"
                className="mb-4"
              />

              <input
                type="password"
                onChange={(ev) =>
                  setAdmin({ ...admin, password: ev.target.value })
                }
                placeholder="Password"
                className="mb-4"
              />
              <input
                type="password"
                onChange={(ev) =>
                  setAdmin({ ...admin, confirm: ev.target.value })
                }
                placeholder="Password Confirmation"
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

export default AdminForm;
