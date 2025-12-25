/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: App.js
 * Descrição: Componente Raiz da aplicação React.
 * Define o Router e gere a navegação entre todas as páginas (Rotas).
 * Implementa a lógica de "Rota Privada" para proteger o acesso ao Dashboard.
 * ===================================================================================
 */

import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import Login from "./pages/Login";
import Register from "./pages/Register";
import CreateTrip from "./pages/CreateTrip";
import Trips from "./pages/Trips";
import Home from "./pages/Home";
import EditTrip from "./pages/EditTrip";

/**
 * Componente de Guarda (Guard Component).
 * Verifica se o utilizador tem um token de autenticação.
 * Se tiver, renderiza a página filha (children).
 * Se não tiver, redireciona para a página inicial (/).
 */
function PrivateRoute({ children }) {
  const token = localStorage.getItem("token");
  return token ? children : <Navigate to="/" />;
}

/**
 * Componente Principal que estrutura a navegação.
 */
function App() {
  return (
    <BrowserRouter>
      <Routes>
        {/* --- ROTAS PÚBLICAS --- */}
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />

        {/* --- ROTAS PROTEGIDAS (Requerem Login) --- */}
        
        {/* Dashboard: Lista de viagens */}
        <Route 
          path="/trips" 
          element={ 
            <PrivateRoute>
              <Trips /> 
            </PrivateRoute>
          } 
        />

        {/* Criar nova viagem */}
        <Route 
            path="/create-trip" 
            element={   
                <PrivateRoute> 
                    <CreateTrip />
                </PrivateRoute>
            }
        />

        {/* Editar viagem existente (recebe o ID na rota) */}
        <Route 
            path="/edit-trip/:id" 
            element={   
                <PrivateRoute> 
                    <EditTrip />
                </PrivateRoute>
            }
        />

      </Routes>
    </BrowserRouter>
  );
}

export default App;