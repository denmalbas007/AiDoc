import axios from "axios";

const API_URL = "http://194.58.119.154:1001/api/v1/";

const getHeaders = () => {
  return {
    Authorization: `Bearer ${localStorage.getItem("jwt")}`,
  };
};

export const doUserSignUp = async (email, fullName, password) => {
  const result = await axios.post(API_URL + "register", {
    email,
    fullName,
    password,
  });

  localStorage.setItem("jwt", result.data.jwtToken);
};

export const doUserSignIn = async (email, password) => {
  const result = await axios.post(API_URL + "auth", {
    email,
    password,
  });

  localStorage.setItem("jwt", result.data.jwtToken);
  return result.data;
};

export const doUploadFile = async (file, progress) => {
  // upload files one by one and update progress
  const formData = new FormData();
  formData.append("file", file);
  const result = await axios.post(API_URL + "documents/analyze", formData, {
    headers: getHeaders(),
    onUploadProgress: (progressEvent) => {
      progress(Math.round((progressEvent.loaded * 100) / progressEvent.total));
    },
  });
  return result.data;
};
