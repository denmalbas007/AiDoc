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
import SignInPage from "./pages/SignInPage";
import SignUpPage from "./pages/SignUpPage";
import Footer from "./components/ui/Footer";
import ReportPage from "./pages/ReportPage";

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
          <Route path="*" element={<Navigate to="/" />} />
          <Route path="/signin" element={<SignInPage />} />
          <Route path="/signup" element={<SignUpPage />} />
          <Route path="/report" element={<ReportPage />} />
        </Routes>
        <Footer />
      </Router>
    </AuthProvider>
  );
}

export default App;
