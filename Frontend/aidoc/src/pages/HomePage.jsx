import React from "react";
import MainNav from "../components/navbars/MainNav";
import { ReactComponent as LogoBigSvg } from "../assets/logo/aidoc_big.svg";
import InfoCard from "../components/ui/cards/InfoCard";
import FileUpload from "../components/fileUpload/FileUpload";

const HomePage = () => {
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
    </main>
  );
};

export default HomePage;
