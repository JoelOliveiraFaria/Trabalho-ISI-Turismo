/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: InsuranceService.cs
 * Descrição: Implementação concreta do serviço SOAP (WCF).
 * Contém a lógica de negócio para calcular o custo do seguro baseado no orçamento.
 * ===================================================================================
 */

using InsuranceSoapService.Interfaces;

namespace InsuranceSoapService.Services
{
    /// <summary>
    /// Classe que implementa a interface do serviço de seguros.
    /// É aqui que reside a lógica de cálculo.
    /// </summary>
    public class InsuranceService : IInsuranceService
    {
        /// <summary>
        /// Calcula o valor do seguro aplicando taxas diferentes consoante o orçamento.
        /// </summary>
        /// <param name="budget">O orçamento total da viagem.</param>
        /// <returns>O valor calculado do seguro.</returns>
        public double CalculateInsurance(double budget)
        {
            // Lógica de cálculo do seguro por escalões
            if (budget < 1000)
            {
                return budget * 0.05; // 5% para orçamentos abaixo de 1000
            }
            else if (budget < 5000)
            {
                return budget * 0.03; // 3% para orçamentos entre 1000 e 5000
            }
            else
            {
                return budget * 0.02; // 2% para orçamentos acima de 5000
            }
        }
    }
}