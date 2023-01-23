import axios from "axios";

const API_URL = "http://194.58.119.154:1001/api/v1/";

const getHeaders = () => {
  return {
    Authorization: `Bearer ${localStorage.getItem("jwt")}`,
  };
};

export const doUserSignUp = async (email, fullName, password) => {
  const result = await axios.post(API_URL + "auth/register", {
    email,
    fullName,
    password,
  });

  localStorage.setItem("jwt", result.data.jwtToken);
  return {
    success: result.status === 200,
  };
};

export const doUserSignIn = async (email, password) => {
  try {
    const result = await axios.post(API_URL + "auth", {
      email,
      password,
    });

    localStorage.setItem("jwt", result.data.jwtToken);
    return {
      success: result.status === 200,
    };
  } catch {
    return {
      success: false,
    };
  }
};

export const doUserSignOut = () => {
  localStorage.removeItem("jwt");
};

export const doCheckAuth = async () => {
  if (localStorage.getItem("jwt")) {
    const result = axios.get(API_URL + "users", {
      headers: getHeaders(),
    });

    return result;
  } else {
    return null;
  }
};

export const doGetHistory = async () => {
  try {
    const result = await axios.get(API_URL + "users/content", {
      headers: getHeaders(),
    });

    return result.data;
  } catch {
    return null;
  }
};

export const doUploadFile = async (file, progress) => {
  return await mockDoUploadFile(file, progress);
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

export const mockDoUploadFile = async (file, progress) => {
  await new Promise((resolve) => setTimeout(resolve, 1000));

  for (let i = 1; i <= 10; i++) {
    progress(i * 10);
    await new Promise((resolve) => setTimeout(resolve, 150));
  }

  await new Promise((resolve) => setTimeout(resolve, 2000));

  const isSell = Math.random() > 0.5;

  const sizeToKb = (size) => {
    return Math.round(size / 1024);
  };

  return {
    id: Math.random(),
    name: file.name,
    size: sizeToKb(file.size) + " кб",
    extension: file.name.split(".").pop(),
    prediction: isSell ? "Договор купли-продажи" : "Договор оказания услуг",
    sentences: isSell
      ? [
          "Договор купли-продажи № 123 от 01.01.2020 г. между ООО «Рога и копыта» и ИП «Иванов Иван Иванович»",
          "Продавец передает и покупатель принимает в полном объеме на свои риски и на условиях, указанных в настоящем договоре, объект купли-продажи.",
          "Покупатель обязуется оплатить продавцу купленное им имущество в соответствии с условиями настоящего договора.",
        ]
      : [
          "Стороны договорились о предоставлении услуг по оказанию технической поддержки.",
          "В соответствии с данным договором, исполнитель обязуется оказывать услуги по содержанию и обслуживанию оборудования.",
          "Заказчик обязуется оплачивать услуги, оказанные исполнителем в соответствии с условиями данного договора оказания услуг.",
          "Настоящий договор оказания услуг имеет силу на весь период его действия и регулирует взаимоотношения между сторонами в отношении оказания указанных услуг.",
        ],
  };
};
