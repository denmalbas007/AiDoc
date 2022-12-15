import React from "react";
import { useState } from "react";
import { ReactComponent as PdfSvg } from "../assets/icons/pdf.svg";
import { ReactComponent as WordSvg } from "../assets/icons/word.svg";

const testReports = [
  {
    id: "1",
    name: "Договор на поставку товаров",
    extension: "wordx",
    size: "56 КБайт, 2 страницы",
    tags: ["Договор", "Поставка", "Товары", "Деньги", "Акции", "Котировки"],
    type: "Договор о покупке",
  },
  {
    id: "2",
    name: "Договор на продажу стульев",
    extension: "pdf",
    size: "423 Байт, 1 страница",
    tags: ["Договор", "Продажа", "Стулья", "Деньги", "Акции", "Котировки"],
    type: "Страховое свидетельство",
  },
];

const ReportPage = () => {
  const [reports, setReports] = useState(testReports);
  console.log(reports);
  return (
    <main className="report-page">
      {reports.map((report) => (
        <div className="report">
          <div className="report__header">
            {report.extension === "pdf" ? <PdfSvg /> : <WordSvg />}
            <div className="report__name">
              <h2>{report.name}</h2>
              <p>
                {report.size} ({report.extension})
              </p>
            </div>
          </div>
          <div className="report__tags">
            <h3>Ключевые теги</h3>
            <div className="report__tags_wrapper">
              {report.tags.map((tag) => (
                <div className="report__tag">{tag}</div>
              ))}
            </div>
          </div>
          <div className="report__type">
            <h3>Вид документа</h3>
            <h2>{report.type}</h2>
          </div>
        </div>
      ))}
    </main>
  );
};

export default ReportPage;
