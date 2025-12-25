/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: IInsuranceService.cs
 * Descrição: Interface que define o contrato para o serviço de cálculo de seguros.
 * Abstrai a lógica de negócio para determinar o custo do seguro de viagem.
 * ===================================================================================
 */

namespace TravelAPI.Interfaces
{
    /// <summary>
    /// Define o contrato para serviços de cálculo de seguros de viagem.
    /// </summary>
    public interface IInsuranceService
    {
        /// <summary>
        /// Calcula o custo do seguro com base no orçamento total da viagem.
        /// </summary>
        /// <param name="budget">O orçamento total estipulado para a viagem.</param>
        /// <returns>O valor calculado do seguro (decimal).</returns>
        Task<decimal> CalculateInsuranceAsync(decimal budget);
    }
}