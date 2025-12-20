import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import Login from "./pages/Login";
import Register from "./pages/Register";
import CreateTrip from "./pages/CreateTrip";
import Trips from "./pages/Trips";

function PrivateRoute({ children }) {
  const token = localStorage.getItem("token");
  return token ? children : <Navigate to="/" />;
}

function App() {
  return (
    <BrowserRouter>
      <Routes>
        {/* Rota Pública: Login */}
        <Route path="/" element={<Login />} />

        <Route path="/register" element={<Register />} />

        {/* Rota Protegida: Viagens (Vamos criar a seguir) */}
        <Route 
          path="/trips" 
          element={ <Trips /> } 
        />

        <Route path="/create-trip" element={<CreateTrip />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;