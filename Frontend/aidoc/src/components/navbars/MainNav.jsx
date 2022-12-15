import React from "react";
import { useLocation } from "react-router-dom";
import { Link } from "react-router-dom";
import { ReactComponent as LogoSvg } from "../../assets/logo/aidoc_small.svg";

const MainNav = () => {
  // get current page
  const location = useLocation();

  return (
    <div className="main_nav">
      <LogoSvg />
      <ul className="links">
        <li className={location.pathname === "/" ? "active" : ""}>
          <Link to="/">Конвертировать</Link>
        </li>
        <li className={location.pathname === "/prices" ? "active" : ""}>
          <Link to="/prices">Цены</Link>
        </li>
        <li className={location.pathname === "/about" ? "active" : ""}>
          <Link to="/about">Помощь</Link>
        </li>
      </ul>
      <button className="btn-outline">Зарегистрироваться</button>
      <button className="btn-primary">Войти</button>
    </div>
  );
};

export default MainNav;
