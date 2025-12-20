import { useEffect, useState } from "react";
import api from "../services/api";
import { Link, useNavigate } from "react-router-dom";
import Navbar from "../pages/Navbar"; // Importar Navbar

function Trips() {
  const [trips, setTrips] = useState([]);
  const [loading, setLoading] = useState(true);
  const userName = localStorage.getItem("userName");
  const navigate = useNavigate();

  const loadTrips = async () => {
    try {
      const response = await api.get("travel/trips");
      setTrips(response.data);
    } catch (error) {
        if(error.response && error.response.status === 401) navigate("/login");
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id) => {
      if (!window.confirm("Apagar viagem?")) return;
      try {
          await api.delete(`travel/trips/${id}`);
          setTrips(trips.filter(t => t.id !== id));
      } catch (e) { alert("Erro ao apagar"); }
  };

  useEffect(() => { loadTrips(); }, []);


  return (
    <div className="min-vh-100 bg-light">
      <Navbar />

      {/* 1. HERO HEADER (Cabeçalho Azul) */}
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

      {/* 2. ÁREA DOS CARTÕES (Com margem negativa para subir) */}
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
                <div key={trip.id} className="col-md-4 mb-4">
                <div className="card h-100 border-0 shadow-sm hover-shadow" style={{transition: "0.3s"}}>
                    {/* Imagem genérica ou cor no topo do card */}
                    <div className="card-header bg-white border-0 pt-4 pb-0">
                        <h4 className="fw-bold text-primary mb-0">{trip.destination.city}</h4>
                        <small className="text-muted">{trip.destination.country}</small>
                    </div>
                    
                    <div className="card-body">
                        <hr className="my-3 opacity-25"/>
                        <p className="mb-2"><strong>📅 </strong> {new Date(trip.startDate).toLocaleDateString()} ➝ {new Date(trip.endDate).toLocaleDateString()}</p>
                        <p className="mb-2"><strong>💰 </strong> {trip.budget} €</p>
                        <p className="mb-2"><strong>⛅ </strong> {trip.weatherForecast}</p>
                        
                        {trip.notes && (
                            <div className="alert alert-light border mt-3 fst-italic text-muted">
                                "{trip.notes}"
                            </div>
                        )}
                    </div>

                    <div className="card-footer bg-white border-0 d-flex justify-content-between align-items-center pb-3">
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