import React from "react";
import { Link } from "react-router-dom";
import { ReactComponent as HelloSvg } from "../assets/icons/hello.svg";
import { doCheckAuth, doUserSignIn } from "../api/Auth";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useContext } from "react";
import { AuthContext } from "../api/AuthContext";

const SignInPage = () => {
  const [errorMessage, setErrorMessage] = useState("");
  const { setUser } = useContext(AuthContext);
  const navigate = useNavigate();

  const onSignIn = async (e) => {
    e.preventDefault();
    const email = e.target[0].value;
    const password = e.target[1].value;
    const result = await doUserSignIn(email, password);
    if (result.success) {
      const user = await doCheckAuth();
      setUser({
        name: user.data.fullName,
        daysLeft: "26 дней",
      });
      navigate("/");
    } else {
      setErrorMessage("Неверный логин или пароль");
    }
  };

  return (
    <div className="sign-holder">
      <div className="sign-content">
        <div className="header">
          <HelloSvg />
          <h3>Аутентификация</h3>
        </div>
        <form className="sign-form" onSubmit={onSignIn}>
          <input
            className="default-input"
            type="email"
            placeholder="Введите почту"
          />
          <input
            className="default-input"
            type="password"
            placeholder="Введите пароль"
          />
          <p className="error">{errorMessage}</p>
          <button className="btn-primary">Войти</button>
          <Link className="link" to="/signup">
            Нет аккаунта?
          </Link>
        </form>
      </div>
    </div>
  );
};

export default SignInPage;
