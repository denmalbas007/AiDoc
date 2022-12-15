import { AuthProvider } from "./api/AuthContext";
import {
  BrowserRouter as Router,
  Navigate,
  Route,
  Routes,
} from "react-router-dom";
import HomePage from "./pages/HomePage";

function App() {
  return (
    <AuthProvider>
      <Router>
        <Routes>
          <Route path="/" element={<HomePage />} />
        </Routes>
      </Router>
    </AuthProvider>
  );
}

export default App;
