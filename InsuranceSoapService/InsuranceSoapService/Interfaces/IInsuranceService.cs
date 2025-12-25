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
 * Descrição: Interface que define o Contrato do Serviço (Service Contract) para o WCF.
 * Especifica as operações que ficarão visíveis no WSDL para os clientes SOAP.
 * ===================================================================================
 */

using System.ServiceModel;

namespace InsuranceSoapService.Interfaces
{
    /// <summary>
    /// Define o contrato do serviço SOAP (WCF).
    /// É através desta interface que os clientes (como a TravelAPI) sabem quais os métodos disponíveis.
    /// </summary>
    [ServiceContract]
    public interface IInsuranceService
    {
        /// <summary>
        /// Operação exposta para calcular o valor do seguro de viagem.
        /// </summary>
        /// <param name="budget">O orçamento total da viagem.</param>
        /// <returns>O valor calculado do seguro (tipo double).</returns>
        [OperationContract]
        double CalculateInsurance(double budget);
    }
}