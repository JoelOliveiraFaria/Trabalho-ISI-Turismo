/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: TripDTO.cs
 * Descrição: Contém os Data Transfer Objects (DTOs) para a entidade Viagem (Trip).
 * Define os modelos de dados para criação de viagens e para apresentação ao cliente.
 * ===================================================================================
 */

using System.ComponentModel.DataAnnotations;

namespace TravelAPI.DTOs
{
    /// <summary>
    /// DTO utilizado para receber os dados de criação ou edição de uma viagem.
    /// </summary>
    public class CreateTripDto
    {
        /// <summary>
        /// ID do destino escolhido para a viagem.
        /// </summary>
        [Required]
        public int DestinationId { get; set; }

        /// <summary>
        /// Data de início da viagem.
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Data de fim da viagem.
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Orçamento definido pelo utilizador.
        /// </summary>
        [Required]
        public decimal Budget { get; set; }

        /// <summary>
        /// Notas ou observações pessoais sobre a viagem.
        /// </summary>
        public string Notes { get; set; }
    }

    /// <summary>
    /// DTO utilizado para apresentar os detalhes completos de uma viagem.
    /// Inclui dados calculados (Seguro) e externos (Meteorologia).
    /// </summary>
    public class TripDto
    {
        /// <summary>
        /// Identificador único da viagem.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Data de início.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Data de fim.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Orçamento estipulado.
        /// </summary>
        public decimal Budget { get; set; }

        /// <summary>
        /// Notas do utilizador.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Custo do seguro (Calculado automaticamente).
        /// </summary>
        public decimal? InsuranceCost { get; set; }

        /// <summary>
        /// Previsão meteorológica (Obtida de serviço externo).
        /// </summary>
        public string? WeatherForecast { get; set; }

        /// <summary>
        /// Detalhes do destino associado (Objeto aninhado).
        /// </summary>
        public DestinationDto Destination { get; set; }
    }
}