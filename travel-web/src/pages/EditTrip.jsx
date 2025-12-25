/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: EditTrip.js
 * Descrição: Componente React responsável pela edição de uma viagem existente.
 * Carrega os dados atuais da API, preenche o formulário e submete alterações (PUT).
 * ===================================================================================
 */

import { useState, useEffect } from "react";
import { useParams, useNavigate, Link } from "react-router-dom";
import api from "../services/api";
import Navbar from "../pages/Navbar";

/**
 * Componente para editar detalhes de uma viagem.
 * Utiliza o ID da rota para buscar os dados originais antes de permitir a edição.
 */
function EditTrip() {
  // Obtém o ID da viagem a partir da URL (ex: /trips/edit/5)
  const { id } = useParams();
  const navigate = useNavigate();

  // Estado para armazenar os campos do formulário
  const [formData, setFormData] = useState({
    destinationId: "",
    startDate: "",
    endDate: "",
    budget: "",
    notes: ""
  });

  /**
   * useEffect: Carrega os dados da viagem assim que o componente monta.
   * Converte as datas para o formato yyyy-MM-dd compatível com o input type="date".
   */
  useEffect(() => {
    const loadTrip = async () => {
      try {
        const response = await api.get(`travel/trips/${id}`);
        const data = response.data;

        setFormData({
          destinationId: data.destination.id,
          // split("T")[0] remove a parte da hora da string ISO (2025-10-10T00:00:00)
          startDate: data.startDate.split("T")[0], 
          endDate: data.endDate.split("T")[0],
          budget: data.budget,
          notes: data.notes || ""
        });
      } catch (error) {
        console.error("Erro ao carregar viagem:", error);
        alert("Erro ao carregar os dados da viagem.");
        navigate("/trips");
      }
    };
    loadTrip();
  }, [id, navigate]);

  /**
   * Atualiza o estado do formulário conforme o utilizador altera os inputs.
   * @param {Event} e - Evento de mudança do input.
   */
  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  /**
   * Submete os dados atualizados para a API via pedido PUT.
   * @param {Event} e - Evento de submissão do formulário.
   */
  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      // Envia as alterações para o backend
      await api.put(`travel/trips/${id}`, {
        ...formData,
        destinationId: parseInt(formData.destinationId),
        budget: parseFloat(formData.budget)
      });
      
      alert("Alterações guardadas com sucesso!");
      navigate("/trips");
    } catch (error) {
      console.error("Erro ao atualizar:", error);
      alert("Ocorreu um erro ao tentar atualizar a viagem.");
    }
  };

  return (
    <div className="min-vh-100 bg-light">
      <Navbar />
      <div className="container mt-5">
        <div className="card shadow p-4 mx-auto" style={{ maxWidth: '600px' }}>
          <h2 className="mb-4 text-center">✏️ Editar Viagem</h2>
          
          <form onSubmit={handleSubmit}>
            {/* Campos de Data */}
            <div className="row">
              <div className="col-md-6 mb-3">
                <label className="form-label">Data de Início</label>
                <input 
                    type="date" 
                    name="startDate" 
                    className="form-control" 
                    value={formData.startDate} 
                    onChange={handleChange} 
                    required 
                />
              </div>
              <div className="col-md-6 mb-3">
                <label className="form-label">Data de Fim</label>
                <input 
                    type="date" 
                    name="endDate" 
                    className="form-control" 
                    value={formData.endDate} 
                    onChange={handleChange} 
                    required 
                />
              </div>
            </div>

            {/* Campo de Orçamento */}
            <div className="mb-3">
              <label className="form-label">Orçamento (€)</label>
              <input 
                type="number" 
                name="budget" 
                className="form-control" 
                value={formData.budget} 
                onChange={handleChange} 
                required 
              />
            </div>

            {/* Campo de Notas */}
            <div className="mb-3">
              <label className="form-label">Notas / Observações</label>
              <textarea 
                name="notes" 
                className="form-control" 
                rows="3" 
                value={formData.notes} 
                onChange={handleChange}
              ></textarea>
            </div>

            {/* Botões de Ação */}
            <div className="d-grid gap-2 d-md-block">
                <button type="submit" className="btn btn-primary me-2">Salvar Alterações</button>
                <Link to="/trips" className="btn btn-secondary">Cancelar</Link>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}

export default EditTrip;