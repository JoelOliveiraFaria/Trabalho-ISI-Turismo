/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: CreateTrip.js
 * Descrição: Componente React responsável pelo formulário de criação de viagens.
 * Permite selecionar o destino, datas e orçamento, comunicando com a API.
 * ===================================================================================
 */

import { useState, useEffect } from "react";
import api from "../services/api";
import { useNavigate } from "react-router-dom";
import Navbar from "../pages/Navbar";

/**
 * Componente de criação de nova viagem.
 * Gere o estado do formulário e submete os dados para a API (POST).
 */
function CreateTrip() {
    const navigate = useNavigate();

    // Estado para armazenar a lista de destinos carregada da API
    const [destination, setDestination] = useState([]);
    
    // Estado do formulário
    const [formData, setFormData] = useState({
        destinationId: '',
        startDate: '',
        endDate: '',
        budget: '',
        notes: ''
    });

    // Estado para mensagens de erro na UI
    const [error, setError] = useState('');

    /**
     * useEffect: Carrega os destinos disponíveis assim que o componente é montado.
     */
    useEffect(() => {
        async function loadDestinations() {
            try {
                const response = await api.get('travel/destinations');
                setDestination(response.data);
            } catch (err) {
                console.error('Erro ao carregar destinos:', err);
                setError('Não foi possível carregar a lista de destinos.');
            }
        }
        loadDestinations();
    }, []);

    /**
     * Atualiza o estado do formulário conforme o utilizador digita.
     * @param {Event} e - Evento de alteração do input.
     */
    const handleChange = (e) => { 
        setFormData({
            ...formData,
            [e.target.name]: e.target.value
        });
    };

    /**
     * Submete o formulário para criar a viagem.
     * Inclui validação de datas e tratamento de avisos (warnings) do backend.
     * @param {Event} e - Evento de submissão.
     */
    const handleSubmit = async (e) => { 
        e.preventDefault();
        setError('');

        try {
            // Validação local: Data de fim não pode ser antes do início
            if (new Date(formData.endDate) < new Date(formData.startDate)) {
                setError('A data de término não pode ser anterior à data de início.');
                return;
            }

            const response = await api.post('travel/trips', {
                destinationId: parseInt(formData.destinationId),
                startDate: formData.startDate,
                endDate: formData.endDate,
                budget: parseFloat(formData.budget),
                notes: formData.notes
            });
            
            // Verifica se o backend devolveu avisos (ex: conflitos de agenda Google Calendar)
            if(response.data.warnings && response.data.warnings.length > 0) { 
                alert('Viagem criada, mas com avisos na agenda:\n' + response.data.warnings.join('\n'));
            } else {
                alert('Viagem criada com sucesso! ✈️');
            }
            
            // Redireciona para a lista de viagens
            navigate('/trips');

        } catch (err) {
            console.error('Erro ao criar viagem:', err);
            setError('Falha ao criar viagem. Verifica os dados e tenta novamente.');
        }
    };
    
    return (
    <div className="min-vh-100 bg-light">
        <Navbar />
        <div className="container py-5">
            <div className="card shadow p-4 mx-auto" style={{maxWidth: '600px'}}>
                <h2 className="mb-4 text-center">✈️ Marcar nova viagem</h2>
                
                {error && <div className="alert alert-danger">{error}</div>}

                <form onSubmit={handleSubmit}>
                    {/* Seleção de Destino */}
                    <div className="mb-3">
                        <label htmlFor="destinationId" className="form-label">Destino</label>
                        <select
                            id="destinationId"
                            name="destinationId"
                            value={formData.destinationId}
                            onChange={handleChange}
                            className="form-select"
                            required
                        >
                            <option value="">Selecione um destino</option>
                            {destination.map(dest => (
                                <option key={dest.id} value={dest.id}>
                                    {dest.city}, {dest.country}
                                </option>
                            ))}
                        </select>
                    </div>

                    {/* Seleção de Datas */}
                    <div className="row">
                        <div className="col-md-6 mb-3">
                            <label htmlFor="startDate" className="form-label">Data de Início</label>
                            <input
                                type="date"
                                id="startDate"
                                name="startDate"
                                value={formData.startDate}
                                onChange={handleChange}
                                className="form-control"
                                required
                            />
                        </div>

                        <div className="col-md-6 mb-3">
                            <label htmlFor="endDate" className="form-label">Data de Término</label>
                            <input
                                type="date"
                                id="endDate"
                                name="endDate"
                                value={formData.endDate}
                                onChange={handleChange}
                                className="form-control"
                                required
                            />
                        </div>
                    </div>

                    {/* Orçamento */}
                    <div className="mb-3">
                        <label htmlFor="budget" className="form-label">Orçamento (EUR €)</label>
                        <input
                            type="number"
                            id="budget"
                            name="budget"
                            value={formData.budget}
                            onChange={handleChange}
                            className="form-control"
                            placeholder="Ex: 1000"
                            required
                        />
                    </div>

                    {/* Observações */}
                    <div className="mb-3">
                        <label htmlFor="notes" className="form-label">Observações</label>
                        <textarea
                            id="notes"
                            name="notes"
                            value={formData.notes}
                            onChange={handleChange}
                            rows={4}
                            className='form-control'
                            placeholder="Ex: Levar protetor solar..."
                        ></textarea>
                    </div>

                    {/* Botões de Ação */}
                    <div className="d-grid gap-2">
                        <button type='submit' className='btn btn-primary'>Criar Viagem</button>
                        <button type="button" className="btn btn-secondary" onClick={() => navigate("/trips")}>
                            Cancelar
                        </button>
                    </div>
                </form>
            </div>
        </div>
    </div>
    );
}

export default CreateTrip;