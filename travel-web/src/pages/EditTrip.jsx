import { useState, useEffect } from "react";
import { useParams, useNavigate, Link } from "react-router-dom";
import api from "../services/api";
import Navbar from "../pages/Navbar";

function EditTrip() {
  const { id } = useParams();
  const navigate = useNavigate();

  // Estado simples para os campos do formulário
  const [formData, setFormData] = useState({
    destinationId: "",
    startDate: "",
    endDate: "",
    budget: "",
    notes: ""
  });

  useEffect(() => {
    const loadTrip = async () => {
      try {
        const response = await api.get(`travel/trips/${id}`);
        const data = response.data;

        setFormData({
          destinationId: data.destination.id,
          startDate: data.startDate.split("T")[0], 
          endDate: data.endDate.split("T")[0],
          budget: data.budget,
          notes: data.notes || ""
        });
      } catch (error) {
        alert("Erro ao carregar viagem.");
        navigate("/");
      }
    };
    loadTrip();
  }, [id, navigate]);

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      // 3. Enviar as alterações
      await api.put(`travel/trips/${id}`, {
        ...formData,
        destinationId: parseInt(formData.destinationId),
        budget: parseFloat(formData.budget)
      });
      alert("Guardado!");
      navigate("/trips");
    } catch (error) {
      alert("Erro ao atualizar.");
    }
  };

  return (
    <div className="min-vh-100 bg-light">
      <Navbar />
      <div className="container mt-5">
        <div className="card shadow p-4">
          <h2 className="mb-4">Editar Viagem</h2>
          
          <form onSubmit={handleSubmit}>


            <div className="row">
              <div className="col-md-6 mb-3">
                <label className="form-label">Início</label>
                <input type="date" name="startDate" className="form-control" 
                       value={formData.startDate} onChange={handleChange} required />
              </div>
              <div className="col-md-6 mb-3">
                <label className="form-label">Fim</label>
                <input type="date" name="endDate" className="form-control" 
                       value={formData.endDate} onChange={handleChange} required />
              </div>
            </div>

            <div className="mb-3">
              <label className="form-label">Orçamento (€)</label>
              <input type="number" name="budget" className="form-control" 
                     value={formData.budget} onChange={handleChange} required />
            </div>

            <div className="mb-3">
              <label className="form-label">Notas</label>
              <textarea name="notes" className="form-control" rows="3" 
                        value={formData.notes} onChange={handleChange}></textarea>
            </div>

            <button type="submit" className="btn btn-primary me-2">Salvar</button>
            <Link to="/trips" className="btn btn-secondary">Cancelar</Link>
          </form>
        </div>
      </div>
    </div>
  );
}

export default EditTrip;