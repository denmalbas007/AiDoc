import React from "react";
import { ReactComponent as LogoBigSvg } from "../assets/logo/aidoc_big.svg";
import { ReactComponent as PdfSvg } from "../assets/icons/pdf.svg";
import { ReactComponent as WordSvg } from "../assets/icons/word.svg";
import InfoCard from "../components/ui/cards/InfoCard";
import FileUpload from "../components/fileUpload/FileUpload";
import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

const testFiles = [
  {
    id: "1",
    name: "договор.docx",
    extension: "wordx",
    fileSize: "56 КБайт",
    uploadProgress: 54,
  },
  {
    id: "2",
    name: "misis.pdf",
    extension: "pdf",
    fileSize: "423 Байт",
    uploadProgress: 100,
  },
];

const HomePage = () => {
  const [uploadedFiles, setUploadedFiles] = useState(testFiles);
  const [uploadedFilesBody, setUploadedFilesBody] = useState([]);
  const navigate = useNavigate();

  const onFileUpload = (files) => {
    // if file extension is pdf, docx, doc, rtf add to uploadedFilesBody
    for (let i = 0; i < files.length; i++) {
      const file = files[i];
      const fileExtension = file.name.split(".").pop();
      if (
        fileExtension === "pdf" ||
        fileExtension === "docx" ||
        fileExtension === "doc" ||
        fileExtension === "rtf"
      ) {
        setUploadedFilesBody((prev) => [...prev, file]);
      }
    }
    console.log(uploadedFilesBody);
  };

  const onUploadFilesToServer = async () => {
    // upload files to server
    const apiUrl = "http://194.58.119.154:1001/api/v1/documents/analyze";
    // upload files one by one and update progress
    for (let i = 0; i < uploadedFilesBody.length; i++) {
      const file = uploadedFilesBody[i];
      const formData = new FormData();
      formData.append("file", file);
      const response = await fetch(apiUrl, {
        method: "POST",
        body: formData,
      });
      const data = await response.json();
      console.log(data);
    }
  };

  const updateFileCards = () => {
    // update uploadedFiles
    // for each file in uploadedFilesBody
    // get file name, extension, file size
    // add to uploadedFiles
    setUploadedFiles([]);
    for (let i = 0; i < uploadedFilesBody.length; i++) {
      const file = uploadedFilesBody[i];
      const fileName = file.name;
      const fileExtension = fileName.split(".").pop();
      const fileSize = Math.round(file.size / 1024) + " КБайт";
      const fileObj = {
        id: i,
        name: fileName,
        extension: fileExtension,
        fileSize: fileSize,
        uploadProgress: 0,
      };
      setUploadedFiles((prev) => [...prev, fileObj]);
    }
  };

  useEffect(() => {
    // updateFileCards();
  }, [uploadedFilesBody]);

  return (
    <main className="home-page">
      <div className="home__header">
        <LogoBigSvg />
        <p>
          Добро пожаловать в сервис,
          <br />
          который поможет вам быстро определить вид договора
          <br />и облегчит процесс маршрутизации внутри компании
        </p>
        <InfoCard>
          Создайте аккаунт, чтобы получить полный доступ к сервису
        </InfoCard>
      </div>
      <div className="upload">
        <FileUpload onFileUpload={onFileUpload} />
      </div>
      <div className="uploaded-files">
        <h2>Процесс загрузки</h2>
        <div className="files_wrapper">
          {uploadedFiles.map((file) => (
            <div key={file.id} className="file">
              <div className="row">
                <div className="file__icon">
                  {file.extension === "pdf" ? <PdfSvg /> : <WordSvg />}
                </div>
                <div className="col">
                  <div className="file__name">{file.name}</div>
                  <div className="file__size">{file.fileSize}</div>
                </div>
              </div>
              <div className="file__progress">
                <div className="file__progress-bar">
                  <div
                    className="file__progress-bar__inner"
                    style={{ width: `${file.uploadProgress}%` }}
                  ></div>
                </div>
              </div>
              <div className="file__status">
                Загрузка документа {file.uploadProgress}%
              </div>
            </div>
          ))}
        </div>
        <div className="button-holder">
          <button className="btn-primary" onClick={() => navigate("/report")}>
            Начать обработку
          </button>
        </div>
      </div>
    </main>
  );
};

export default HomePage;
