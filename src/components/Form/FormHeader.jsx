import React from "react";

function FormHeader({formName,data,id}) {
  return (
    <div className="w-full h-20  justify-between items-center inline-flex">
      <div className="text-black text-5xl font-bold font-['Roboto'] leading-[62.40px]">
        {id && <h1>Update {formName}: {data.name} </h1>}
        {!id && <h1>New {formName}</h1>}
      </div>
    </div>
  );
}

export default FormHeader;
