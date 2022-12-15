import React from "react";
import { useState } from "react";
import { ReactComponent as UploadSvg } from "../../assets/icons/upload.svg";

const FileUpload = () => {
  const [drag, setDrag] = useState(false);

  const dragStartHandler = (e) => {
    e.preventDefault();
    setDrag(true);
  };

  const dragLeaveHandler = (e) => {
    e.preventDefault();
    setDrag(false);
  };

  const onDropHandler = (e) => {
    e.preventDefault();
    let files = [...e.dataTransfer.files];
    console.log(files);
    setDrag(false);
  };

  const fileUploadHandler = (e) => {
    let files = [...e.target.files];
    console.log(files);
  };

  const onRequestFileUpload = () => {
    document.getElementById("file").click();
  };

  return (
    <div className="file-upload">
      <div className="header">
        <h2>Загрузите документ</h2>
        <p>
          Для того, чтобы загрузить новые данные для расчёта, вставьте в форму
          ниже файл в формате .doc, .docx или .pdf{" "}
        </p>
      </div>
      <div
        className="drag-area"
        onDragStart={(e) => dragStartHandler(e)}
        onDragLeave={(e) => dragLeaveHandler(e)}
        onDragOver={(e) => dragStartHandler(e)}
        onDrop={(e) => onDropHandler(e)}
      >
        {/* Allowed files: pdf, docx, doc */}
        <input
          type="file"
          accept=".pdf, .docx, .doc"
          id="file"
          multiple
          onChange={(e) => fileUploadHandler(e)}
        />
        <UploadSvg />
        <label htmlFor="file">
          Перетащите файл в формате .doc, .docx или .pdf сюда
        </label>
      </div>
    </div>
  );
};

export default FileUpload;
