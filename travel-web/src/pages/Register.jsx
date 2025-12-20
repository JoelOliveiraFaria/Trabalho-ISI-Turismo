import {useState} from 'react';
import { useNavigate, Link } from "react-router-dom";
import api from "../services/api.jsx";


function Register() {

    const [username, setUsername] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');

    const navigate = useNavigate();

    const handleRegister = async (e) => {
        e.preventDefault();
        setError('');

        try {
            await api.post('travel/user/register', {
                email : email,
                password : password,
            })

            alert('Registro realizado com sucesso! Faça o login.');
            navigate('/');

        } catch (err) {
            setError('Falha no registro. Tente novamente.');
            console.error(err);
            return;
        }
    }

return (
    
    <div className="container d-flex justify-content-center align-items-center vh-100">
      <div className="card p-4 shadow" style={{ width: "450px" }}>
        <h2 className="text-center mb-4">Criar Conta</h2>
        
        {error && <div className="alert alert-danger">{error}</div>}

        <form onSubmit={handleRegister}>
          
          {/* Campo Username */}
          <div className="mb-3">
            <label className="form-label">Username</label>
            <input 
              type="text" 
              className="form-control" 
              required 
              value={username} onChange={(e) => setUsername(e.target.value)}
            />
          </div>

          {/* Campo Email */}
          <div className="mb-3">
            <label className="form-label">Email</label>
            <input 
              type="email" 
              className="form-control" 
              required 
              value={email} 
              onChange={(e) => setEmail(e.target.value)}
            />
          </div>
          
          {/* Campo Password */}
          <div className="mb-3">
            <label className="form-label">Password</label>
            <input 
              type="password" 
              className="form-control" 
              required 
              value={password} 
              onChange={(e) => setPassword(e.target.value)}
            />
          </div>

          <div className="d-grid gap-2">
            <button type="submit" className="btn btn-success">Registar</button>
            
            <Link to="/" className="btn btn-outline-secondary">
              Já tenho conta (Login)
            </Link>
          </div>
        </form>
      </div>
    </div>
  );
}

export default Register;