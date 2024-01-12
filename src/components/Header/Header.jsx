import React from 'react';
import { Link } from 'react-router-dom';

function Header({ title, isButtonNew }) {
  return (
    <div className="w-full h-20  justify-between items-center inline-flex">
      <div className="text-black text-5xl font-bold font-['Roboto'] leading-[62.40px]">
        {title}
      </div>
      {isButtonNew  ? (
        <div className="flex justify-center items-center">
          <Link
            to="create"
            className="w-[81px] px-3.5 py-2 bg-emerald-600 rounded-lg shadow justify-center items-center gap-2 flex"
          >
            <div className="text-white text-xs font-bold font-['Roboto'] uppercase leading-[18px]">
              new
            </div>
          </Link>
        </div>
      ) : null}
    </div>
  );
}

export default Header;
