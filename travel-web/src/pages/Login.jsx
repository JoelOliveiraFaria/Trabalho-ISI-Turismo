import {useState} from 'react';
import { jwtDecode } from "jwt-decode";
import { useNavigate, Link } from "react-router-dom";
import api from "../services/api.jsx";

function Login() {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();
    
    const handleLogin = async (e) => {
        e.preventDefault();
        setError('');

        try {
            const response = await api.post("travel/user/login", {
                name : username,
                username : username,
                password : password,
            });

            const token = response.data;

            const decodedToken = jwtDecode(token);
            console.log("Dados do Token:", decodedToken);

            const userId = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
            const userName = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
            const userEmail = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"];

            localStorage.setItem("token", token);
            localStorage.setItem("userId", userId);
            localStorage.setItem("userName", userName);
            localStorage.setItem("userEmail", userEmail);

            navigate("/trips");


        } catch (err) {
            setError('Falha no login. Verifique suas credenciais.');
            console.error(err);
        }

    };

    return(
        <div className='container d-flex justify-content-center align-items-center vh-100'>
            <div className='card p-4 shadow' style={{width: '400px'}}>
                <h2 className='text-center mb-4'>Login</h2>
                {error && <div className='alert alert-danger'>{error}</div>}

                <form onSubmit={handleLogin}>
                    <div className='mb-3'>
                        <label className='form-label'>Username</label>
                        <input
                            type="text"
                            className='form-control'
                            value={username}
                            onChange={(e) => setUsername(e.target.value)}
                            required
                        />
                    </div>

                    <div className='mb-3'>
                        <label className='form-label'>Password</label>
                        <input
                            type="password"
                            className='form-control'
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            required
                        />
                    </div>

                    <div className="d-grid gap-2">
                        <button type="submit" className='btn btn-primary w-100'>Login</button>

                        <Link to="/register" className="btn btn-outline-secondary">
                            Não tenho conta (Registar)
                        </Link>
                    </div>
                </form>
            </div>
        </div>
    );
}

export default Login;