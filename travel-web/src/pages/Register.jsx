import { useState } from 'react';
import { useNavigate, Link } from "react-router-dom";
import api from "../services/api.jsx";
import Navbar from "../pages/Navbar"; // <--- Importante: Importar a Navbar

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
            // Nota: Adicionei 'name: username' para garantir que o backend aceita 
            // (caso a tabela Users obrigue a ter Nome)
            await api.post('travel/user/register', {
                name: username, 
                username: username,
                email: email,
                password: password,
            });

            alert('Registo realizado com sucesso! Faça o login.');
            navigate('/login'); 

        } catch (err) {
            if (err.response && err.response.data) {
                const mensagemDoServidor = typeof err.response.data === 'string' 
                        ? err.response.data 
                        : JSON.stringify(err.response.data);

                setError(mensagemDoServidor);
            } else {
                setError('Falha no registo. Tente novamente.');
                console.error('Erro inesperado:', err);
            }
        }
    }

    return (
        <div className="d-flex flex-column min-vh-100" style={{ background: 'linear-gradient(135deg, #0d6efd 0%, #f8f9fa 100%)' }}>
            
            <Navbar />

            <div className="flex-grow-1 d-flex align-items-center justify-content-center">
                <div className="card p-5 shadow-lg border-0" style={{ width: "450px", borderRadius: "15px" }}>
                    
                    {/* Cabeçalho do Card */}
                    <div className="text-center mb-4">
                        <h1 style={{ fontSize: "3rem" }}>🚀</h1>
                        <h3 className="fw-bold text-dark">Criar Conta</h3>
                        <p className="text-muted">Junta-te a nós e começa a planear!</p>
                    </div>

                    {error && <div className="alert alert-danger">{error}</div>}

                    <form onSubmit={handleRegister}>
                        
                        {/* Campo Username */}
                        <div className="mb-3">
                            <label className="form-label fw-bold text-secondary">Username</label>
                            <input 
                                type="text" 
                                className="form-control form-control-lg bg-light" 
                                required 
                                value={username} onChange={(e) => setUsername(e.target.value)}
                            />
                        </div>

                        {/* Campo Email */}
                        <div className="mb-3">
                            <label className="form-label fw-bold text-secondary">Email</label>
                            <input 
                                type="email" 
                                className="form-control form-control-lg bg-light" 
                                required 
                                value={email} 
                                onChange={(e) => setEmail(e.target.value)}
                            />
                        </div>
                        
                        {/* Campo Password */}
                        <div className="mb-4">
                            <label className="form-label fw-bold text-secondary">Password</label>
                            <input 
                                type="password" 
                                className="form-control form-control-lg bg-light" 
                                required 
                                value={password} 
                                onChange={(e) => setPassword(e.target.value)}
                            />
                        </div>

                        {/* Botão de Registar */}
                        <button type="submit" className="btn btn-primary btn-lg w-100 mb-3 shadow-sm">
                            Registar
                        </button>
                        
                        {/* Link para Login */}
                        <div className="text-center">
                            <Link to="/login" className="text-decoration-none text-muted">
                                Já tens conta? <span className="text-primary fw-bold">Faz Login</span>
                            </Link>
                        </div>

                    </form>
                </div>
            </div>
        </div>
    );
}

export default Register;