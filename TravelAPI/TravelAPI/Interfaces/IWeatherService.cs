/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: IWeatherService.cs
 * Descrição: Interface que define o contrato para o serviço de meteorologia.
 * Permite obter previsões do tempo para cidades específicas.
 * ===================================================================================
 */

namespace TravelAPI.Interfaces
{
    /// <summary>
    /// Define o contrato para serviços de informação meteorológica.
    /// </summary>
    public interface IWeatherService
    {
        /// <summary>
        /// Obtém a previsão meteorológica atual para uma cidade específica.
        /// </summary>
        /// <param name="city">Nome da cidade a pesquisar.</param>
        /// <returns>Uma string com a descrição do tempo (ex: "Sol, 25ºC").</returns>
        Task<string> GetWeatherAsync(string city);
    }
}