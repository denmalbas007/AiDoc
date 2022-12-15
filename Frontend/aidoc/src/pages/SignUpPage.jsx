import React from "react";
import { Link } from "react-router-dom";
import { ReactComponent as HelloSvg } from "../assets/icons/hello.svg";

const SignUpPage = () => {
  return (
    <div className="sign-holder">
      <div className="sign-content">
        <div className="header">
          <HelloSvg />
          <h3>Регистрация</h3>
        </div>
        <form className="sign-form">
          <input
            className="default-input"
            type="email"
            placeholder="Введите почту"
            required
          />
          <input
            className="default-input"
            type="text"
            placeholder="Введите фамилию и имя"
            required
          />
          <input
            className="default-input"
            type="password"
            placeholder="Введите пароль"
            required
          />
          <input
            className="default-input"
            type="password"
            placeholder="Повторите пароль"
            required
          />
          <button className="btn-primary">Зарегистрироваться</button>
          <Link className="link" to="/signin">
            Уже есть аккаунт?
          </Link>
        </form>
      </div>
    </div>
  );
};

export default SignUpPage;
