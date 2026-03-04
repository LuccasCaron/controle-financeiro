using System.Reflection;

namespace ControleFinanceiro.Application;

/// <summary>
/// Classe de referência para o assembly da Application.
/// </summary>
public static class ApplicationAssemblyReference
{
    public static Assembly Assembly => typeof(ApplicationAssemblyReference).Assembly;
}
