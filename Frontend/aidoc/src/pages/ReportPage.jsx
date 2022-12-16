import React from "react";
import { useState } from "react";
import { ReactComponent as PdfSvg } from "../assets/icons/pdf.svg";
import { ReactComponent as WordSvg } from "../assets/icons/word.svg";

const testReports = [
  {
    id: "1",
    name: "0b4be82b86eff410d69d1d6b5553d220.docx",
    extension: "docx",
    size: "56 КБайт, 2 страницы",
    tags: [
      "Права",
      "Обязанности",
      "Основания",
      "Адреса",
      "Реквизиты",
      "Исполнитель",
      "Заказчик",
      "Обучающийся",
    ],
    type: "Договоры оказания услуг",
  },
  {
    id: "2",
    name: "4a5707e447271a188a1211606b158a94.pdf",
    extension: "pdf",
    size: "423 Байт, 1 страница",
    tags: ["Штраф", "Товар", "Продавец", "Адреса", "Ответственность", "Адрес"],
    type: "Договоры купли-продажи",
  },
  {
    id: "3",
    name: "0ca2f9faecdbc67d6686a9f5b6636eba.doс",
    extension: "doc",
    size: "423 Байт, 1 страница",
    tags: ["Покупатель", "Поставщик", "Товар", "Деньги", "Акции", "Котировки"],
    type: "Договоры купли-продажи",
  },
  {
    id: "4",
    name: "2b25ecf601a9ce0c2a33c8e1d9746df2.doc",
    extension: "doc",
    size: "423 Байт, 1 страница",
    tags: ["Недвижимость", "Паспорт", "Ответственность", "Положения"],
    type: "Договоры аренды",
  },
  {
    id: "5",
    name: "2b25ecf601a9ce0c2a33c8e1d9746df2.doc",
    extension: "doc",
    size: "423 Байт, 1 страница",
    tags: ["Арендодатель", "Аренду", "Срок", "Плата", "Арендатор"],
    type: "Договоры аренды",
  },
  {
    id: "6",
    name: "2b408919fd9833dc3f6892aad753f21f.docx",
    extension: "docx",
    size: "423 Байт, 1 страница",
    tags: ["Работы", "Подряд", "Стулья", "Приемка", "Подрядчик", "Субподряд"],
    type: "Договоры подряда",
  },
  {
    id: "7",
    name: "2c81df29db63aebf495106881a52188f.doc",
    extension: "dox",
    size: "423 Байт, 1 страница",
    tags: ["Товар", "Продажа", "Ассортимент", "Расчет", "Покупатель", "Счет"],
    type: "Договоры купли-продажи",
  },
  {
    id: "8",
    name: "2c758805e2917306e6cbb079e2adcfcf.rtf",
    extension: "rtf",
    size: "423 Байт, 1 страница",
    tags: [
      "Исполнитель",
      "Услуги",
      "Платные",
      "Плата",
      "Оплачивать",
      "Выполнение",
    ],
    type: "Договоры оказания услуг",
  },
  {
    id: "9",
    name: "2d4708c800dc003466fa9a8a64e2e2b7.docx",
    extension: "docx",
    size: "423 Байт, 1 страница",
    tags: ["Арендодатель", "Аренду", "Дом", "Плата", "Сумма", "Срок"],
    type: "Договоры аренды",
  },
  {
    id: "10",
    name: "2fd747f38e30ae7ce1c9d6e3b907ac5d.doc",
    extension: "doc",
    size: "423 Байт, 1 страница",
    tags: ["Покупатель", "Поставка", "Поставщик", "Приемка", "Акции", "Партия"],
    type: "Договоры поставки",
  },
  {
    id: "1",
    name: "0b4be82b86eff410d69d1d6b5553d220.docx",
    extension: "docx",
    size: "56 КБайт, 2 страницы",
    tags: [
      "Права",
      "Обязанности",
      "Основания",
      "Адреса",
      "Реквизиты",
      "Исполнитель",
      "Заказчик",
      "Обучающийся",
    ],
    type: "Договоры оказания услуг",
  },
  {
    id: "2",
    name: "4a5707e447271a188a1211606b158a94.pdf",
    extension: "pdf",
    size: "423 Байт, 1 страница",
    tags: ["Штраф", "Товар", "Продавец", "Адреса", "Ответственность", "Адрес"],
    type: "Договоры купли-продажи",
  },
  {
    id: "3",
    name: "0ca2f9faecdbc67d6686a9f5b6636eba.doс",
    extension: "doc",
    size: "423 Байт, 1 страница",
    tags: ["Покупатель", "Поставщик", "Товар", "Деньги", "Акции", "Котировки"],
    type: "Договоры купли-продажи",
  },
  {
    id: "4",
    name: "2b25ecf601a9ce0c2a33c8e1d9746df2.doc",
    extension: "doc",
    size: "423 Байт, 1 страница",
    tags: ["Недвижимость", "Паспорт", "Ответственность", "Положения"],
    type: "Договоры аренды",
  },
  {
    id: "5",
    name: "2b25ecf601a9ce0c2a33c8e1d9746df2.doc",
    extension: "doc",
    size: "423 Байт, 1 страница",
    tags: ["Арендодатель", "Аренду", "Срок", "Плата", "Арендатор"],
    type: "Договоры аренды",
  },
  {
    id: "6",
    name: "2b408919fd9833dc3f6892aad753f21f.docx",
    extension: "docx",
    size: "423 Байт, 1 страница",
    tags: ["Работы", "Подряд", "Стулья", "Приемка", "Подрядчик", "Субподряд"],
    type: "Договоры подряда",
  },
  {
    id: "7",
    name: "2c81df29db63aebf495106881a52188f.doc",
    extension: "dox",
    size: "423 Байт, 1 страница",
    tags: ["Товар", "Продажа", "Ассортимент", "Расчет", "Покупатель", "Счет"],
    type: "Договоры купли-продажи",
  },
  {
    id: "8",
    name: "2c758805e2917306e6cbb079e2adcfcf.rtf",
    extension: "rtf",
    size: "423 Байт, 1 страница",
    tags: [
      "Исполнитель",
      "Услуги",
      "Платные",
      "Плата",
      "Оплачивать",
      "Выполнение",
    ],
    type: "Договоры оказания услуг",
  },
  {
    id: "9",
    name: "2d4708c800dc003466fa9a8a64e2e2b7.docx",
    extension: "docx",
    size: "423 Байт, 1 страница",
    tags: ["Арендодатель", "Аренду", "Дом", "Плата", "Сумма", "Срок"],
    type: "Договоры аренды",
  },
  {
    id: "10",
    name: "2fd747f38e30ae7ce1c9d6e3b907ac5d.doc",
    extension: "doc",
    size: "423 Байт, 1 страница",
    tags: ["Покупатель", "Поставка", "Поставщик", "Приемка", "Акции", "Партия"],
    type: "Договоры поставки",
  },
  {
    id: "1",
    name: "0b4be82b86eff410d69d1d6b5553d220.docx",
    extension: "docx",
    size: "56 КБайт, 2 страницы",
    tags: [
      "Права",
      "Обязанности",
      "Основания",
      "Адреса",
      "Реквизиты",
      "Исполнитель",
      "Заказчик",
      "Обучающийся",
    ],
    type: "Договоры оказания услуг",
  },
  {
    id: "2",
    name: "4a5707e447271a188a1211606b158a94.pdf",
    extension: "pdf",
    size: "423 Байт, 1 страница",
    tags: ["Штраф", "Товар", "Продавец", "Адреса", "Ответственность", "Адрес"],
    type: "Договоры купли-продажи",
  },
  {
    id: "3",
    name: "0ca2f9faecdbc67d6686a9f5b6636eba.doс",
    extension: "doc",
    size: "423 Байт, 1 страница",
    tags: ["Покупатель", "Поставщик", "Товар", "Деньги", "Акции", "Котировки"],
    type: "Договоры купли-продажи",
  },
  {
    id: "4",
    name: "2b25ecf601a9ce0c2a33c8e1d9746df2.doc",
    extension: "doc",
    size: "423 Байт, 1 страница",
    tags: ["Недвижимость", "Паспорт", "Ответственность", "Положения"],
    type: "Договоры аренды",
  },
];

const ReportPage = () => {
  const [reports, setReports] = useState(testReports);
  console.log(reports);
  return (
    <main className="report-page">
      {reports.map((report, index) => (
        <div className="report" key={index}>
          <div className="report__header">
            {report.extension === "pdf" ? <PdfSvg /> : <WordSvg />}
            <div className="report__name">
              <h2>{report.name}</h2>
              <p>
                {
                  // set size as random number from 100 to 2000
                  Math.floor(Math.random() * 2000) + 100 + " КБайт"
                }{" "}
                ({report.extension})
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
