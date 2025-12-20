import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import Login from "./pages/Login";
import Register from "./pages/Register";

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
          element={
            <PrivateRoute>
              <h1 className="text-center mt-5">Bem-vindo às Viagens! (Em construção...) 🚧</h1>
            </PrivateRoute>
          } 
        />
      </Routes>
    </BrowserRouter>
  );
}

export default App;