/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: Destination.cs
 * Descrição: Modelo de dados (Entidade) que representa um Destino Turístico.
 * Mapeado para a tabela "Destinations" na base de dados.
 * ===================================================================================
 */

using System.ComponentModel.DataAnnotations;

namespace TravelAPI.Models
{
    /// <summary>
    /// Representa um local geográfico (Cidade/País) que pode ser visitado.
    /// </summary>
    public class Destination
    {
        /// <summary>
        /// Chave primária do destino.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome da cidade (Obrigatório).
        /// </summary>
        [Required]
        public string? City { get; set; }

        /// <summary>
        /// Nome do país (Obrigatório).
        /// </summary>
        [Required]
        public string? Country { get; set; }

        /// <summary>
        /// Descrição detalhada sobre o destino.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Propriedade de navegação: Lista de viagens marcadas para este destino.
        /// Relação: Um Destino pode ter muitas Viagens (1-N).
        /// </summary>
        public List<Trip> Trips { get; set; }
    }
}