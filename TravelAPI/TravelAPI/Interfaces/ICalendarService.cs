/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: ICalendarService.cs
 * Descrição: Interface que define o contrato para o serviço de calendário.
 * Permite a abstração da verificação de conflitos de agenda.
 * ===================================================================================
 */

namespace TravelAPI.Interfaces
{
    /// <summary>
    /// Define o contrato para serviços de gestão de calendário e agenda.
    /// </summary>
    public interface ICalendarService
    {
        /// <summary>
        /// Verifica se existem conflitos de agenda num determinado intervalo de datas.
        /// </summary>
        /// <param name="start">Data de início do intervalo a verificar.</param>
        /// <param name="end">Data de fim do intervalo a verificar.</param>
        /// <returns>Uma lista de strings descrevendo os conflitos encontrados (vazia se não houver conflitos).</returns>
        Task<List<string>> CheckConflictsAsync(DateTime start, DateTime end);
    }
}