import React from "react";
import { useContext } from "react";
import { useNavigate } from "react-router-dom";
import { useLocation } from "react-router-dom";
import { Link } from "react-router-dom";
import { AuthContext } from "../../api/AuthContext";
import { ReactComponent as LogoSvg } from "../../assets/logo/aidoc_small.svg";
import { ReactComponent as LogoutSvg } from "../../assets/icons/logout.svg";
import { useEffect } from "react";
import { useState } from "react";
import { doCheckAuth, doUserSignOut } from "../../api/Auth";

const MainNav = () => {
  const [user, setUser] = useState();
  const location = useLocation();
  const navigate = useNavigate();
  const context = useContext(AuthContext);

  useEffect(() => {
    setUser(context.user);
  }, [context.user]);

  useEffect(() => {
    doCheckAuth().then((newUser) => {
      if (newUser) {
        context.setUser({
          name: newUser.data.fullName,
          daysLeft: "26 дней",
        });
      }
    });
  }, []);

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
              className={location.pathname === "/history" ? "active" : ""}
              to="/history"
            >
              Хранилище
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
        </ul>
        {user ? (
          <div className="account authorized">
            <button
              className="btn-icon"
              onClick={() => {
                doUserSignOut();
                context.setUser(null);
                navigate("/signin");
              }}
            >
              <LogoutSvg />
            </button>
            <div className="account_info">
              <div className="account_name">{user.name}</div>
              <div className="account_days">{user.daysLeft}</div>
            </div>
            <div className="avatar">
              <img
                src={
                  user.avatar ? user.avatar : "https://via.placeholder.com/44"
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
