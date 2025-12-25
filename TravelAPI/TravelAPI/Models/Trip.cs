/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: Trip.cs
 * Descrição: Modelo de dados (Entidade) que representa uma Viagem.
 * Esta é a entidade central que agrega o Utilizador, o Destino e dados externos
 * (Previsão do tempo e Custo do seguro).
 * ===================================================================================
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAPI.Models
{
    /// <summary>
    /// Representa uma viagem planeada por um utilizador.
    /// Entidade central que liga Utilizadores a Destinos.
    /// </summary>
    public class Trip
    {
        /// <summary>
        /// Chave primária da viagem.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Data de início da viagem (Obrigatório).
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Data de fim da viagem (Obrigatório).
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Orçamento estipulado para a viagem.
        /// Define-se o tipo decimal(18,2) para suportar valores monetários corretamente na BD.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Budget { get; set; }

        /// <summary>
        /// Notas ou apontamentos livres sobre a viagem.
        /// </summary>
        public string? Notes { get; set; }

        // --- CAMPOS PARA INTEGRAÇÃO (SOAP & REST Externo) ---

        /// <summary>
        /// Custo do seguro de viagem.
        /// Valor calculado automaticamente via integração com serviço SOAP (InsuranceService).
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? InsuranceCost { get; set; }

        /// <summary>
        /// Previsão ou estado do tempo para o destino.
        /// Valor obtido via integração com API REST externa (WeatherService).
        /// </summary>
        public string? WeatherForecast { get; set; }

        // --- RELAÇÕES (Foreign Keys) ---

        /// <summary>
        /// Chave Estrangeira para o Utilizador que criou a viagem.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Propriedade de navegação para o objeto User.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Chave Estrangeira para o Destino escolhido.
        /// </summary>
        public int DestinationId { get; set; }

        /// <summary>
        /// Propriedade de navegação para o objeto Destination.
        /// </summary>
        public Destination Destination { get; set; }
    }
}