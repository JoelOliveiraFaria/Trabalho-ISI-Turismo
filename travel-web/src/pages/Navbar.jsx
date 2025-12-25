/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: Navbar.js
 * Descrição: Componente de navegação transversal a toda a aplicação.
 * Verifica a autenticação para mostrar opções de Login ou Logout/Dashboard.
 * ===================================================================================
 */

import { Link, useNavigate } from "react-router-dom";

/**
 * Barra de navegação superior (Menu).
 * Responsável pela navegação principal e pela gestão da ação de Logout.
 */
function Navbar() {
  const navigate = useNavigate();
  
  // Verifica se existe um token guardado para determinar o estado de autenticação
  const token = localStorage.getItem("token");

  /**
   * Efetua o logout do utilizador.
   * Limpa todo o LocalStorage (Token, ID, Username) e redireciona para o Login.
   */
  const handleLogout = () => {
    localStorage.clear();
    navigate("/login");
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-primary shadow-sm px-4">
      <div className="container">
        {/* Brand / Logo */}
        <Link className="navbar-brand fw-bold" to="/">
          ✈️ Travel Planner
        </Link>
        
        <div className="d-flex gap-2">
          {/* Renderização Condicional: Se tem token, mostra menu de utilizador */}
          {token ? (
            <>
              <Link to="/trips" className="btn btn-light text-primary fw-bold me-2">
                Minhas Viagens
              </Link>
              <button onClick={handleLogout} className="btn btn-outline-light">
                Sair
              </button>
            </>
          ) : (
            /* Se não tem token, mostra botão de entrar */
            <Link to="/login" className="btn btn-light text-primary">Entrar</Link>
          )}
        </div>
      </div>
    </nav>
  );
}

export default Navbar;