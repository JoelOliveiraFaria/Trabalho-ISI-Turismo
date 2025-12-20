import { useState } from "react";
import api from "../services/api";
import { useNavigate, Link } from "react-router-dom";
import { jwtDecode } from "jwt-decode";
import Navbar from "../pages/Navbar"; 

function Login() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    setError("");
    try {
      const response = await api.post("travel/user/login", { username, password });
      const token = response.data;
      const decoded = jwtDecode(token);
      
      localStorage.setItem("token", token);
      localStorage.setItem("userId", decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]);
      localStorage.setItem("userName", decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"]);

      navigate("/trips"); 
    } catch (err) {
      setError("Login falhou! Verifica as credenciais.");
    }
  };

  return (
    // 1. Fundo Gradiente que ocupa a altura toda (min-vh-100)
    <div className="d-flex flex-column min-vh-100" style={{ background: 'linear-gradient(135deg, #0d6efd 0%, #f8f9fa 100%)' }}>
      
      <Navbar /> {/* Barra de navegação */}

      <div className="flex-grow-1 d-flex align-items-center justify-content-center">
        <div className="card p-5 shadow-lg border-0" style={{ width: "400px", borderRadius: "15px" }}>
          <div className="text-center mb-4">
            <h1 style={{ fontSize: "3rem" }}>🌍</h1>
            <h3 className="fw-bold text-dark">Bem-vindo de volta</h3>
            <p className="text-muted">Planeia a tua próxima aventura</p>
          </div>
          
          {error && <div className="alert alert-danger">{error}</div>}

          <form onSubmit={handleLogin}>
            <div className="mb-3">
              <label className="form-label fw-bold text-secondary">Username</label>
              <input type="text" className="form-control form-control-lg bg-light" value={username} onChange={(e) => setUsername(e.target.value)} required />
            </div>
            
            <div className="mb-4">
              <label className="form-label fw-bold text-secondary">Password</label>
              <input type="password" className="form-control form-control-lg bg-light" value={password} onChange={(e) => setPassword(e.target.value)} required />
            </div>

            <button type="submit" className="btn btn-primary btn-lg w-100 mb-3 shadow-sm">Entrar</button>
            
            <div className="text-center">
              <Link to="/register" className="text-decoration-none text-muted">
                Ainda não tens conta? <span className="text-primary fw-bold">Regista-te</span>
              </Link>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}

export default Login;