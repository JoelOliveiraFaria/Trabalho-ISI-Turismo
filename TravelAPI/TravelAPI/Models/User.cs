/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: User.cs
 * Descrição: Modelo de dados (Entidade) que representa um Utilizador da plataforma.
 * Armazena as credenciais de acesso e mantém a relação com as viagens criadas.
 * ===================================================================================
 */

using System.ComponentModel.DataAnnotations;

namespace TravelAPI.Models
{
    /// <summary>
    /// Representa um utilizador registado na aplicação.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Chave primária do utilizador.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome de utilizador (Login).
        /// </summary>
        [Required]
        public string? Username { get; set; }

        /// <summary>
        /// Endereço de email (usado para notificações).
        /// O atributo [EmailAddress] valida automaticamente o formato.
        /// </summary>
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        /// <summary>
        /// Hash da palavra-passe (encriptada).
        /// A password original nunca é guardada em texto limpo.
        /// </summary>
        [Required]
        public string? PasswordHash { get; set; }

        /// <summary>
        /// Propriedade de navegação: Lista de viagens criadas por este utilizador.
        /// Relação: Um Utilizador pode ter muitas Viagens (1-N).
        /// </summary>
        public List<Trip> Trips { get; set; }
    }
}