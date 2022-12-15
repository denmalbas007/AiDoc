import React from "react";
import { useContext } from "react";
import { useNavigate } from "react-router-dom";
import { useLocation } from "react-router-dom";
import { Link } from "react-router-dom";
import { AuthContext } from "../../api/AuthContext";
import { ReactComponent as LogoSvg } from "../../assets/logo/aidoc_small.svg";
import { ReactComponent as LogoutSvg } from "../../assets/icons/logout.svg";

const MainNav = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const context = useContext(AuthContext);

  return (
    <nav className="main_nav">
      <div className="content">
        <Link to="/">
          <LogoSvg />
        </Link>
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
        {context.user ? (
          <div className="account authorized">
            <button className="btn-icon">
              <LogoutSvg />
            </button>
            <div className="account_info">
              <div className="account_name">{context.user.name}</div>
              <div className="account_days">{context.user.daysLeft}</div>
            </div>
            <div className="avatar">
              <img
                src={
                  context.user.avatar
                    ? context.user.avatar
                    : "https://via.placeholder.com/44"
                }
                alt="avatar"
              />
            </div>
          </div>
        ) : (
          <div className="account">
            <button className="btn-outline" onClick={() => navigate("/signup")}>
              Зарегистрироваться
            </button>
            <button className="btn-primary" onClick={() => navigate("/signin")}>
              Войти
            </button>
          </div>
        )}
      </div>
    </nav>
  );
};

export default MainNav;
