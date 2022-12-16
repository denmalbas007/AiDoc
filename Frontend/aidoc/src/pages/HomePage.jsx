import React from "react";
import { ReactComponent as LogoBigSvg } from "../assets/logo/aidoc_big.svg";
import { ReactComponent as PdfSvg } from "../assets/icons/pdf.svg";
import { ReactComponent as WordSvg } from "../assets/icons/word.svg";
import { ReactComponent as DeleteSvg } from "../assets/icons/delete.svg";
import InfoCard from "../components/ui/cards/InfoCard";
import FileUpload from "../components/fileUpload/FileUpload";
import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { doUploadFile } from "../api/Auth";
import Carousel from "../components/ui/Carousel";
import Checkmark from "../components/ui/Checkmark";

const HomePage = () => {
  const [uploadedFiles, setUploadedFiles] = useState([]);
  const [uploadedFilesBody, setUploadedFilesBody] = useState([]);
  const [uploading, setUploading] = useState(false);
  const [uploadSuccess, setUploadSuccess] = useState(false);
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

  const onFileDelete = (id) => {
    const updatedFiles = uploadedFiles.filter((file) => file.id !== id);
    setUploadedFiles(updatedFiles);
    // delete by index
    const updatedFilesBody = uploadedFilesBody.filter((file, index) => {
      return index !== id;
    });
    setUploadedFilesBody(updatedFilesBody);
  };

  const onUploadFilesToServer = async () => {
    setUploading(true);
    for (let i = 0; i < uploadedFilesBody.length; i++) {
      const result = await doUploadFile(uploadedFilesBody[i], (progress) =>
        updateProgress(i, progress)
      );
      console.log(result);
    }
    onUploadSuccess();
  };

  const updateProgress = (id, progress) => {
    const updatedFiles = uploadedFiles.map((file) => {
      if (file.id === id) {
        file.uploadProgress = progress;
      }
      return file;
    });
    setUploadedFiles(updatedFiles);
  };

  const updateFileCards = () => {
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

  const onUploadSuccess = () => {
    setUploadSuccess(true);
    setTimeout(() => {
      navigate("/report");
    }, 1500);
  };

  useEffect(() => {
    updateFileCards();
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
      {!uploading && (
        <div className="upload">
          <FileUpload onFileUpload={onFileUpload} />
        </div>
      )}
      {uploadedFiles.length > 0 && (
        <div className="uploaded-files">
          <Carousel currentPage={uploadSuccess ? 1 : 0}>
            <div className="files-holder">
              <h2>Процесс загрузки</h2>
              <div className="files-wrapper">
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
                      {!uploading && (
                        <div
                          className="file__delete"
                          onClick={() => onFileDelete(file.id)}
                        >
                          <DeleteSvg />
                        </div>
                      )}
                    </div>
                    {uploading && (
                      <>
                        <div className="file__progress">
                          <div className="file__progress-bar">
                            <div
                              className="file__progress-bar__inner"
                              style={{ width: `${file.uploadProgress}%` }}
                            ></div>
                          </div>
                        </div>
                        <div className="file__status">
                          {file.uploadProgress === 100
                            ? "Загрузка завершена"
                            : "Загрузка документа"}
                          {file.uploadProgress === 100 ? "" : " "}
                          {file.uploadProgress === 100
                            ? ""
                            : file.uploadProgress}
                          {file.uploadProgress === 100 ? "" : "%"}
                        </div>
                      </>
                    )}
                  </div>
                ))}
              </div>
              <div className="button-holder">
                <button className="btn-primary" onClick={onUploadFilesToServer}>
                  Начать обработку
                </button>
              </div>
            </div>
            <div className="upload-success">
              {uploadSuccess && <Checkmark size={48} />}
              <h2>Обработка успешна</h2>
            </div>
          </Carousel>
        </div>
      )}
    </main>
  );
};

export default HomePage;
