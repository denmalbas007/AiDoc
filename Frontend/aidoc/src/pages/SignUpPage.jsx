import React from "react";
import { Link } from "react-router-dom";
import { ReactComponent as HelloSvg } from "../assets/icons/hello.svg";
import { doCheckAuth, doUserSignIn, doUserSignUp } from "../api/Auth";
import { useState, useContext } from "react";
import { AuthContext } from "../api/AuthContext";
import { useNavigate } from "react-router-dom";

const SignUpPage = () => {
  const [errorMessage, setErrorMessage] = useState("");
  const { setUser } = useContext(AuthContext);

  const navigate = useNavigate();
  const onSignUp = async (e) => {
    e.preventDefault();
    const email = e.target[0].value;
    const fullName = e.target[1].value;
    const password = e.target[2].value;
    const password2 = e.target[3].value;
    if (password !== password2) {
      setErrorMessage("Пароли не совпадают");
      return;
    }
    const fullNameParts = fullName.split(" ");
    if (fullNameParts.length !== 2) {
      setErrorMessage("Введите фамилию и имя");
      return;
    }

    const response = await doUserSignUp(email, fullName, password);
    if (response.success) {
      const auth = doCheckAuth();
      if (auth) {
        setUser(auth);
        navigate("/");
      }
    } else {
      setErrorMessage(response.data.message);
    }
  };
  return (
    <div className="sign-holder">
      <div className="sign-content">
        <div className="header">
          <HelloSvg />
          <h3>Регистрация</h3>
        </div>
        <form className="sign-form" onSubmit={onSignUp}>
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
          <p className="error">{errorMessage}</p>
          <button className="btn-primary" onClick={() => navigate("/signin")}>
            Зарегистрироваться
          </button>
          <Link className="link" to="/signin">
            Уже есть аккаунт?
          </Link>
        </form>
      </div>
    </div>
  );
};

export default SignUpPage;
