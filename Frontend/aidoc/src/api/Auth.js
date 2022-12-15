import axios from "axios";

const API_URL = "http://localhost:3000/api/auth/";

const getHeaders = () => {
  return {
    Authorization: `Bearer ${localStorage.getItem("jwt")}`,
  };
};
