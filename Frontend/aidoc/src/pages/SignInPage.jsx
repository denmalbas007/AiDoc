import React from "react";
import { Link } from "react-router-dom";
import { ReactComponent as HelloSvg } from "../assets/icons/hello.svg";

const SignInPage = () => {
  return (
    <div className="sign-holder">
      <div className="sign-content">
        <div className="header">
          <HelloSvg />
          <h3>Аутентификация</h3>
        </div>
        <form className="sign-form">
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
