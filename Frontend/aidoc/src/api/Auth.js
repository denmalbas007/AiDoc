import axios from "axios";

const API_URL = "http://194.58.119.154:1001/api/v1/";

const getHeaders = () => {
  return {
    Authorization: `Bearer ${localStorage.getItem("jwt")}`,
  };
};

export const doUserSignUp = (email, fullName, password) => {
  return axios.post(API_URL + "register", {
    email,
    fullName,
    password,
  });
};

export const doUserSignIn = (email, password) => {
  return axios.post(API_URL + "login", {
    email,
    password,
  });
};
