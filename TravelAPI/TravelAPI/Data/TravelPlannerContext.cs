/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: TravelPlannerContext.cs
 * Descrição: Contexto da base de dados (Entity Framework Core).
 * Define as tabelas (DbSets) e as configurações de relacionamentos entre entidades.
 * ===================================================================================
 */

using Microsoft.EntityFrameworkCore;
using TravelAPI.Models;

namespace TravelAPI.Data
{
    /// <summary>
    /// Classe de Contexto da Base de Dados.
    /// Faz a ponte entre os modelos C# e as tabelas SQL.
    /// </summary>
    public class TravelPlannerContext : DbContext
    {
        public TravelPlannerContext(DbContextOptions<TravelPlannerContext> options) : base(options) { }

        /// <summary>
        /// Tabela de Utilizadores registados na plataforma.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Tabela de Destinos turísticos disponíveis.
        /// </summary>
        public DbSet<Destination> Destinations { get; set; }

        /// <summary>
        /// Tabela de Viagens planeadas pelos utilizadores.
        /// </summary>
        public DbSet<Trip> Trips { get; set; }
    }
}
