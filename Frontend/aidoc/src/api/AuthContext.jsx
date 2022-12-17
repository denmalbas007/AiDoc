import { useState } from "react";
import { createContext } from "react";
import { doCheckAuth } from "./Auth";

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(
    null
    // doCheckAuth()
  );
  const [readyReports, setReadyReports] = useState([]);

  return (
    <AuthContext.Provider
      value={{
        user,
        setUser,
        readyReports,
        setReadyReports,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};
