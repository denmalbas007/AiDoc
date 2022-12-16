import React from "react";
import { ReactComponent as LogoBigSvg } from "../assets/logo/aidoc_big.svg";
import { ReactComponent as PdfSvg } from "../assets/icons/pdf.svg";
import { ReactComponent as WordSvg } from "../assets/icons/word.svg";
import InfoCard from "../components/ui/cards/InfoCard";
import FileUpload from "../components/fileUpload/FileUpload";
import { useState } from "react";
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
    name: "misisAiSucks.pdf",
    extension: "pdf",
    fileSize: "423 Байт",
    uploadProgress: 100,
  },
];

const HomePage = () => {
  const [uploadedFiles, setUploadedFiles] = useState(testFiles);
  const navigate = useNavigate();

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
        <FileUpload />
      </div>
      <div className="uploaded-files">
        <h2>Процесс загрузки</h2>
        <div className="files_wrapper">
          {uploadedFiles.map((file) => (
            <div className="file">
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
