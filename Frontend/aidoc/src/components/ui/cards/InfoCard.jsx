import React from "react";
import { ReactComponent as InfoIcon } from "../../../assets/icons/info.svg";

const InfoCard = ({ children }) => {
  return (
    <div className="info-card">
      <InfoIcon />
      <p>{children}</p>
    </div>
  );
};

export default InfoCard;
