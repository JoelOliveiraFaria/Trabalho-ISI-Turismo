/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: DestinationDTO.cs
 * Descrição: Contém os Data Transfer Objects (DTOs) para a entidade Destino.
 * Separa os dados de entrada (criação/edição) dos dados de saída (leitura).
 * ===================================================================================
 */

using System.ComponentModel.DataAnnotations;

namespace TravelAPI.DTOs
{
    /// <summary>
    /// DTO utilizado para receber dados de criação ou edição de um destino.
    /// Contém validações obrigatórias.
    /// </summary>
    public class CreateDestinationDto
    {
        /// <summary>
        /// Nome da cidade do destino (Obrigatório).
        /// </summary>
        [Required]
        public string? City { get; set; }

        /// <summary>
        /// Nome do país onde se situa a cidade (Obrigatório).
        /// </summary>
        [Required]
        public string? Country { get; set; }

        /// <summary>
        /// Descrição breve ou detalhes turísticos sobre o local.
        /// </summary>
        public string? Description { get; set; }
    }

    /// <summary>
    /// DTO utilizado para enviar os dados de um destino para o cliente (Frontend).
    /// Inclui o ID da base de dados.
    /// </summary>
    public class DestinationDto
    {
        /// <summary>
        /// Identificador único do destino.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome da cidade.
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// Nome do país.
        /// </summary>
        public string? Country { get; set; }

        /// <summary>
        /// Descrição do destino.
        /// </summary>
        public string? Description { get; set; }
    }
}
