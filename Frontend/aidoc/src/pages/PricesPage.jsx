import React from "react";

const PricesPage = () => {
  return (
    <div className="prices-page">
      <h1 className="title">Тарифные планы</h1>
      <div className="prices">
        <div className="price">
          <h2 className="price__title">Бесплатный</h2>
          <h1 className="price__amount">3</h1>
          <p className="price__description">
            Lorem ipsum dolor sit amet consectetur adipisicing elit. Quisquam
          </p>
          {/* <ul className="price_benefits">
            <li className="price_benefit">Lorem ipsum dolor sit amet.</li>
          </ul> */}
          <button className="btn-outline">Выбрать</button>
        </div>
        <div className="price recommended">
          <h2 className="price__title">Базовый Аккаунт</h2>
          <h1 className="price__amount">3 в день</h1>
          <p className="price__description">
            Информация о тарифе будет в скором времени. Пока что это просто
            красивая карточка.
          </p>
          {/* <ul className="price_benefits">
            <li className="price_benefit">Lorem ipsum dolor sit amet.</li>
          </ul> */}
          <button className="btn-primary">Выбрать</button>
        </div>
        <div className="price">
          <h2 className="price__title">Премиум Аккаунт</h2>
          <h1 className="price__amount">Безлимит</h1>
          <p className="price__description">
            Lorem ipsum dolor sit amet consectetur adipisicing elit. Quisquam
          </p>
          {/* <ul className="price_benefits">
            <li className="price_benefit">Lorem ipsum dolor sit amet.</li>
          </ul> */}
          <button className="btn-outline">Выбрать</button>
        </div>
      </div>
    </div>
  );
};

export default PricesPage;
