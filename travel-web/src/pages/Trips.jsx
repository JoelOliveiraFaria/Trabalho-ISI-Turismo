import { use, useEffect, useState } from "react";
import api from "../services/api";
import { useNavigate, Link } from "react-router-dom";

function Trips() {
    const [trips, setTrips] = useState([]);
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();

    const userName = localStorage.getItem("userName");

    const loadTrips = async () => {
        try {
            const response = await api.get('travel/trips');
            setTrips(response.data);
            setLoading(false);
        } catch (err) {
            console.error("Erro ao carregar viagens:", err);

            if (err.response && err.response.status === 401) {
                handleLogout();
            }
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        loadTrips();
    }, []);

    const handleDelete = async (tripId) => {
        const confirm = window.confirm("Tem certeza que deseja deletar esta viagem?");
        if (!confirm) return;

        try {
            await api.delete(`travel/trips/${tripId}`);
            setTrips(trips.filter(trip => trip.id !== tripId));
        } catch (err) {
            console.error("Erro ao deletar viagem:", err);
        }
    };

    const handleLogout = () => {
        localStorage.clear();
        navigate("/");
    };

return (
    <div className="container mt-5">
      {/* Cabeçalho */}
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h1>👋 Olá, {userName}!</h1>
        <button className="btn btn-outline-danger btn-sm" onClick={handleLogout}>Sair</button>
      </div>

      {/* Título e Botão Criar */}
      <div className="d-flex justify-content-between align-items-center">
        <h3>As tuas Viagens</h3>
        <Link to="/create-trip" className="btn btn-success">
          + Nova Viagem
        </Link>
      </div>
      <hr />

      {/* Listagem dos Cartões */}
      {loading ? (
        <div className="text-center mt-5">
            <div className="spinner-border text-primary"></div>
        </div>
      ) : trips.length === 0 ? (
        <div className="alert alert-info text-center">
            Ainda não tens viagens marcadas. Clica no botão verde para começar! ✈️
        </div>
      ) : (
        <div className="row">
          {trips.map((trip) => (
            <div key={trip.id} className="col-md-4 mb-4">
              <div className="card shadow-sm h-100">
                <div className="card-header bg-primary text-white d-flex justify-content-between align-items-center">
                  <span className="fw-bold">{trip.destination.city}, {trip.destination.country}</span>
                </div>
                
                <div className="card-body">
                  <p><strong>Data:</strong> {new Date(trip.startDate).toLocaleDateString()} a {new Date(trip.endDate).toLocaleDateString()}</p>
                  <p><strong> Orçamento:</strong> {trip.budget} €</p>
                  <p><strong> Previsão:</strong> {trip.weatherForecast}</p>
                  <p className="card-text text-muted fst-italic">"{trip.notes}"</p>
                </div>

                <div className="card-footer bg-white d-flex justify-content-between align-items-center">
                    <small className="text-success fw-bold">Seguro: {trip.insuranceCost} €</small>
                    <button 
                        className="btn btn-danger btn-sm" 
                        onClick={() => handleDelete(trip.id)}
                    >
                         Eliminar
                    </button>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default Trips;