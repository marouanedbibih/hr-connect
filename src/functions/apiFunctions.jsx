import React from 'react'
import axiosClient from '../api/axios'
import { useNavigate } from 'react-router-dom';
import { useStateContext } from '../context/ContextProvider';



function apiFunctions({sectionName}) {
    const navigate = useNavigate();
    const { setNotification } = useStateContext();

    const onDeleteClick = (id) => {
        if (
          !window.confirm("Are you sure you want to delete this Departement !!")
        ) {
          return;
        }
        axiosClient
          .delete(`/job/${id}`)
          .then(() => {
            setNotification("Departement was successfully deleted");
            getJobs(currentPage);
          })
          .catch((error) => {
            console.error(error);
          });
      };


      
  return (
    <div>apiFunctions</div>
  )
}

export default apiFunctions