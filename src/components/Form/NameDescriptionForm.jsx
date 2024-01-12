import React from "react";

function NameDescriptionForm({onSubmit}) {
  return (
    <form onSubmit={onSubmit} className="grid grid-cols-1 gap-4 ">
      <div className="grid grid-colms-1 gab-8 ">
        <input
          value={departement.name}
          onChange={(ev) =>
            setDepartement({ ...departement, name: ev.target.value })
          }
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
  );
}

export default NameDescriptionForm;
