import React from "react";
import { useEffect } from "react";
import { useContext } from "react";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { doGetHistory } from "../api/Auth";
import { AuthContext } from "../api/AuthContext";
import { ReactComponent as PdfSvg } from "../assets/icons/pdf.svg";
import { ReactComponent as WordSvg } from "../assets/icons/word.svg";

const HistoryPage = () => {
  const [reports, setReports] = useState([]);
  const [isAuthorized, setIsAuthorized] = useState(true);
  const [searchText, setSearchText] = useState("");
  const navigate = useNavigate();

  const expandReport = (id) => {
    const newReports = reports.map((report) => {
      if (report.id === id) {
        report.expanded = !report.expanded;
      }
      return report;
    });
    setReports(newReports);
  };

  const getReports = async () => {
    const result = await doGetHistory();
    console.log(result);
    if (result) {
      const newReports = result.documentMeta.map((report) => {
        report.expanded = false;
        report.extension = report.name.split(".").pop();
        return report;
      });
      // reverse
      newReports.reverse();
      setReports(newReports);
    } else {
      setIsAuthorized(false);
    }
  };

  useEffect(() => {
    if (searchText.length > 0) {
      const newReports = reports.map((report) => {
        report.hidden = false;
        if (report.name.includes(searchText)) {
          report.expanded = true;
        } else {
          report.hidden = true;
          report.expanded = false;
        }
        return report;
      });
      setReports(newReports);
    } else {
      const newReports = reports.map((report) => {
        report.hidden = false;

        report.expanded = false;
        return report;
      });
      setReports(newReports);
    }
  }, [searchText]);

  useEffect(() => {
    getReports();
  }, []);
  return (
    <main className="report-page">
      <div className="report-page__header">
        <h1>Хранилище</h1>
        <div className="search">
          <input
            type="text"
            placeholder="Поиск"
            value={searchText}
            onChange={(e) => setSearchText(e.target.value)}
          />
        </div>
      </div>

      {isAuthorized ? (
        // if no reports or all reports hidden
        reports.length === 0 || reports.every((report) => report.hidden) ? (
          <div className="authorize">
            <div className="authorize__header">
              <h3>Документы не найдены</h3>
            </div>
          </div>
        ) : (
          reports.map((report, index) =>
            !report.hidden ? (
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
                <ul className="report__sentences expanded">
                  {report.sentences.length > 0 && (
                    <div className="group">
                      <li className="group__title">
                        <h5>Предложение 1</h5>
                      </li>
                      <li className="group__sentence">{report.sentences[0]}</li>
                    </div>
                  )}
                </ul>

                <ul
                  className={[
                    "report__sentences",
                    report.expanded ? "expanded" : "",
                  ].join(" ")}
                >
                  {report.sentences.slice(1).map((sentence, index) => (
                    <div key={index} className="group">
                      {/* заголовок */}
                      <li className="group__title">
                        <h5>Предложение {index + 2}</h5>
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
            ) : null
          )
        )
      ) : (
        <div className="authorize">
          <div className="authorize__header">
            <h3>Для просмотра документов необходимо войти в аккаунт</h3>
          </div>
          <div className="authorize__body">
            <button className="btn-primary" onClick={() => navigate("/signin")}>
              Войти
            </button>
          </div>
        </div>
      )}
    </main>
  );
};

export default HistoryPage;
