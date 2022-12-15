import React from "react";
import { useLocation } from "react-router-dom";
import { Link } from "react-router-dom";
import { ReactComponent as LogoSvg } from "../../assets/logo/aidoc_small.svg";

const MainNav = () => {
  const location = useLocation();

  return (
    <div className="main_nav">
      <div className="content">
        <LogoSvg />
        <ul className="links">
          <li>
            <Link className={location.pathname === "/" ? "active" : ""} to="/">
              Конвертировать
            </Link>
          </li>
          <li>
            <Link
              className={location.pathname === "/prices" ? "active" : ""}
              to="/prices"
            >
              Цены
            </Link>
          </li>
          <li>
            <Link
              className={location.pathname === "/about" ? "active" : ""}
              to="/about"
            >
              Помощь
            </Link>
          </li>
        </ul>
        <div className="account">
          <button className="btn-outline">Зарегистрироваться</button>
          <button className="btn-primary">Войти</button>
        </div>
      </div>
    </div>
  );
};

export default MainNav;
