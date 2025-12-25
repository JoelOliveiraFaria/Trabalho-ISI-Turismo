/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: Trips.js
 * Descrição: Dashboard principal da aplicação.
 * Lista todas as viagens do utilizador autenticado, apresentando dados da BD local
 * e dados obtidos via integrações (Previsão do tempo e Custo do seguro).
 * ===================================================================================
 */

import { useEffect, useState } from "react";
import api from "../services/api";
import { Link, useNavigate } from "react-router-dom";
import Navbar from "../pages/Navbar";

/**
 * Componente que exibe a lista de viagens (Dashboard).
 * Permite visualizar detalhes, apagar viagens e navegar para edição/criação.
 */
function Trips() {
  const [trips, setTrips] = useState([]);
  const [loading, setLoading] = useState(true);
  
  // Recupera o nome do utilizador do localStorage para personalizar a saudação
  const userName = localStorage.getItem("userName");
  const navigate = useNavigate();

  /**
   * Carrega a lista de viagens da API.
   * Se o token for inválido (401), redireciona para o login.
   */
  const loadTrips = async () => {
    try {
      const response = await api.get("travel/trips");
      setTrips(response.data);
    } catch (error) {
        // Se não autorizado, forçar login
        if(error.response && error.response.status === 401) navigate("/login");
    } finally {
      setLoading(false);
    }
  };

  /**
   * Apaga uma viagem específica.
   * Solicita confirmação ao utilizador antes de contactar a API.
   * @param {number} id - Identificador da viagem.
   */
  const handleDelete = async (id) => {
      if (!window.confirm("Apagar viagem?")) return;
      try {
          await api.delete(`travel/trips/${id}`);
          // Atualiza o estado local removendo o item apagado (evita novo reload)
          setTrips(trips.filter(t => t.id !== id));
      } catch (e) { alert("Erro ao apagar"); }
  };

  /**
   * Navega para a página de edição.
   * @param {number} id - Identificador da viagem.
   */
  const handleEdit = (id) => {
    navigate(`/edit-trip/${id}`);
  }

  // Executa o carregamento inicial ao montar o componente
  useEffect(() => { loadTrips(); }, []);


  return (
    <div className="min-vh-100 bg-light">
      <Navbar />
      
      {/* 1. HEADER / SAUDAÇÃO */}
      <div className="bg-primary text-white py-5 mb-5">
        <div className="container">
          <div className="row align-items-center">
            <div className="col-md-8">
              <h1 className="display-5 fw-bold">Olá, {userName}! </h1>
              <p className="lead opacity-75">Aqui estão as tuas próximas aventuras.</p>
            </div>
            <div className="col-md-4 text-md-end">
              <Link to="/create-trip" className="btn btn-light btn-lg text-primary fw-bold shadow">
                + Nova Viagem
              </Link>
            </div>
          </div>
        </div>
      </div>

      {/* 2. ÁREA DOS CARTÕES (Com margem negativa para efeito visual sobreposto) */}
      <div className="container" style={{ marginTop: "-2rem" }}>
        
        {loading ? (
            <div className="text-center mt-5"><div className="spinner-border text-primary"></div></div>
        ) : trips.length === 0 ? (
            <div className="card shadow-sm p-5 text-center border-0">
                <h3>Ainda não tens viagens 😢</h3>
                <p>Clica no botão em cima para começares a planear!</p>
            </div>
        ) : (
            <div className="row">
            {trips.map((trip) => (
                <div key={trip.id} className="col-md-4 mb-4" >
                <div className="card h-100 border-0 shadow-sm hover-shadow" style={{transition: "0.3s"}}>
                    
                    {/* Cabeçalho do Card: Destino */}
                    <div className="card-header bg-white border-0 pt-4 pb-0">
                        <h4 className="fw-bold text-primary mb-0" onClick={() => handleEdit(trip.id)} style={{cursor: "pointer"}}>
                            {trip.destination.city}
                        </h4>
                        <small className="text-muted">{trip.destination.country}</small>
                    </div>
                    
                    {/* Corpo do Card: Detalhes e Integrações */}
                    <div className="card-body">
                        <hr className="my-3 opacity-25"/>
                        <p className="mb-2"><strong>📅 </strong> {new Date(trip.startDate).toLocaleDateString()} ➝ {new Date(trip.endDate).toLocaleDateString()}</p>
                        <p className="mb-2"><strong>💰 </strong> {trip.budget} €</p>
                        
                        {/* Integração REST: Meteorologia */}
                        <p className="mb-2"><strong>⛅ </strong> {trip.weatherForecast}</p>
                        
                        {trip.notes && (
                            <div className="alert alert-light border mt-3 fst-italic text-muted">
                                "{trip.notes}"
                            </div>
                        )}
                    </div>

                    {/* Rodapé do Card: Seguro e Botão Apagar */}
                    <div className="card-footer bg-white border-0 d-flex justify-content-between align-items-center pb-3">
                        {/* Integração SOAP: Custo do Seguro */}
                        <span className="badge bg-success bg-opacity-10 text-success p-2">
                            Seguro: {trip.insuranceCost} €
                        </span>
                        
                        <button onClick={() => handleDelete(trip.id)} className="btn btn-outline-danger btn-sm rounded-circle" title="Apagar" style={{width: "35px", height: "35px"}}>
                            🗑️
                        </button>
                    </div>
                </div>
                </div>
            ))}
            </div>
        )}
      </div>
    </div>
  );
}

export default Trips;