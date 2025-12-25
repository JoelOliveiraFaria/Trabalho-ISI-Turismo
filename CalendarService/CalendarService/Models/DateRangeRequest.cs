/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: DateRangeRequest.cs
 * Descrição: Modelo de dados (DTO) utilizado para transportar o intervalo de datas
 * nos pedidos HTTP para o serviço de Calendário.
 * ===================================================================================
 */

namespace CalendarService.Models
{
    /// <summary>
    /// Objeto de pedido (Request Payload) que define um intervalo temporal.
    /// Utilizado para verificar disponibilidade ou conflitos entre duas datas.
    /// </summary>
    public class DateRangeRequest
    {
        /// <summary>
        /// Data e hora de início do intervalo.
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// Data e hora de fim do intervalo.
        /// </summary>
        public DateTime End { get; set; }
    }
}