import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import axiosClient from '../../api/axios.js';
import { useStateContext } from '../../context/ContextProvider.jsx';

function SelectForm({datas,loading,select, setSelect,onSubmit}) {

  
    return (
      <>
        <div className="w-1/2 card animated fadeInDown">
          {loading && <div className="text-center">Loading...</div>}
  
          {!loading && (
            <form onSubmit={onSubmit} className="grid grid-cols-1 gap-4">
              <div className="grid grid-cols-1 gap-8">
                <select
                  value={select.value || ""}
                  onChange={(ev) =>
                    setSelect({ ...select, value: ev.target.value })
                  }
                  className="mb-4"
                >
                  <option value="" disabled>Select a job</option>
                  {datas.map((d) => (
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

export default SelectForm