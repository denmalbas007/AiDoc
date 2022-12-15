import { AuthProvider } from "./api/AuthContext";
import {
  BrowserRouter as Router,
  Navigate,
  Route,
  Routes,
} from "react-router-dom";
import HomePage from "./pages/HomePage";
import MainNav from "./components/navbars/MainNav";
import PricesPage from "./pages/PricesPage";
import AboutPage from "./pages/AboutPage";

function App() {
  // always display <MainNav />
  return (
    <AuthProvider>
      <Router>
        <MainNav />
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/prices" element={<PricesPage />} />
          <Route path="/about" element={<AboutPage />} />
        </Routes>
      </Router>
    </AuthProvider>
  );
}

export default App;
