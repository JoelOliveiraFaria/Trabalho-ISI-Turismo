/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: UserDTO.cs
 * Descrição: Contém os Data Transfer Objects (DTOs) para a entidade Utilizador (User).
 * Utilizados para operações de registo, login e apresentação de dados públicos.
 * ===================================================================================
 */

using System.ComponentModel.DataAnnotations;

namespace TravelAPI.DTOs
{
    /// <summary>
    /// DTO utilizado para o registo de novos utilizadores.
    /// </summary>
    public class UserRegisterDTO
    {
        /// <summary>
        /// Nome de utilizador desejado (Obrigatório).
        /// </summary>
        [Required]
        public string? Username { get; set; }

        /// <summary>
        /// Endereço de email do utilizador (Obrigatório).
        /// </summary>
        [Required]
        public string? Email { get; set; }

        /// <summary>
        /// Palavra-passe em texto limpo (será encriptada no servidor).
        /// </summary>
        [Required]
        public string? Password { get; set; }
    }

    /// <summary>
    /// DTO utilizado para autenticação (Login).
    /// </summary>
    public class UserLoginDTO
    {
        /// <summary>
        /// Nome de utilizador para login.
        /// </summary>
        [Required]
        public string? Username { get; set; }

        /// <summary>
        /// Palavra-passe para validação.
        /// </summary>
        [Required]
        public string? Password { get; set; }
    }

    /// <summary>
    /// DTO para retornar dados públicos do utilizador (sem password).
    /// </summary>
    public class UserDTO
    {
        /// <summary>
        /// Identificador único do utilizador na BD.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome de utilizador.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Email do utilizador.
        /// </summary>
        public string? Email { get; set; }
    }
}