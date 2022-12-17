import React from "react";
import { useEffect } from "react";
import { useContext } from "react";
import { useState } from "react";
import { AuthContext } from "../api/AuthContext";
import { ReactComponent as PdfSvg } from "../assets/icons/pdf.svg";
import { ReactComponent as WordSvg } from "../assets/icons/word.svg";

const ReportPage = () => {
  const [reports, setReports] = useState([]);
  const context = useContext(AuthContext);

  const expandReport = (id) => {
    const newReports = reports.map((report) => {
      if (report.id === id) {
        report.expanded = !report.expanded;
      }
      return report;
    });
    setReports(newReports);
  };

  useEffect(() => {
    setReports(context.readyReports);
    console.log(context.readyReports);
  }, [context.readyReports]);

  return (
    <main className="report-page">
      {reports.map((report, index) => (
        <div className="report" key={index}>
          <div className="report__header">
            {report.extension === "pdf" ? <PdfSvg /> : <WordSvg />}
            <div className="report__name">
              <h3>{report.name}</h3>
              <p>
                {report.size} ({report.extension})
              </p>
            </div>
          </div>
          <ul
            className={[
              "report__sentences",
              report.expanded ? "expanded" : "",
            ].join(" ")}
          >
            {report.sentences.map((sentence, index) => (
              <div key={index} className="group">
                {/* заголовок */}
                <li className="group__title">
                  <h5>Предложение {index + 1}</h5>
                </li>
                <li className="group__sentence">{sentence}</li>
              </div>
            ))}
          </ul>
          <div className="report__type">
            <div className="col">
              <h5>Вид документа</h5>
              <h3>{report.prediction}</h3>
            </div>
            <div className="row">
              <button className="btn-outline">Предпросмотр</button>
              <button
                className="btn-primary"
                onClick={() => expandReport(report.id)}
              >
                {report.expanded ? "Свернуть" : "Подробнее"}
              </button>
            </div>
          </div>
        </div>
      ))}
    </main>
  );
};

export default ReportPage;
