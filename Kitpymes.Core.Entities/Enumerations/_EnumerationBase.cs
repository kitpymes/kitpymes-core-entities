// -----------------------------------------------------------------------
// <copyright file="_EnumerationBase.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.Entities.Enumerations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/*
    Clase base de enumeraciones EnumerationBase
    Contiene las acciones comunes a todas las enumeraciones
*/

/// <summary>
/// Clase base de enumeraciones <c>EnumerationBase</c>.
/// Contiene las acciones comunes a todas las enumeraciones.
/// </summary>
/// <remarks>
/// <para>En esta clase se pueden agregar todas las acciones necesarias para las enumeraciones.</para>
/// </remarks>
public abstract class EnumerationBase : IComparable
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="EnumerationBase"/>.
    /// </summary>
    /// <param name="id">Id de la enumeración.</param>
    /// <param name="name">Nombre de la enumeración.</param>
    /// <param name="shortName">Nombre corto de la enumeración.</param>
    protected EnumerationBase(int id, string name, string? shortName = null)
        => (Id, Name, ShortName) = (id, name, shortName);

    /// <summary>
    /// Obtiene el id de la enumeración.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Obtiene el nombre de la enumeración.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Obtiene el nombre corto de la enumeración.
    /// </summary>
    public string? ShortName { get; }

    #region Operator

    /// <summary>
    /// operator.
    /// </summary>
    /// <param name="left">Left.</param>
    /// <param name="right">Rigth.</param>
    /// <returns>bool.</returns>
    public static bool operator ==(EnumerationBase? left, EnumerationBase? right)
    {
        if (left is null)
        {
            return right is null;
        }

        return left.Equals(right);
    }

    /// <summary>
    /// operator.
    /// </summary>
    /// <param name="left">Left.</param>
    /// <param name="right">Rigth.</param>
    /// <returns>bool.</returns>
    public static bool operator !=(EnumerationBase? left, EnumerationBase? right)
    => !(left == right);

    /// <summary>
    /// operator.
    /// </summary>
    /// <param name="left">Left.</param>
    /// <param name="right">Rigth.</param>
    /// <returns>bool.</returns>
    public static bool operator <(EnumerationBase? left, EnumerationBase? right)
    => left is null ? right is not null : left.CompareTo(right) < 0;

    /// <summary>
    /// operator.
    /// </summary>
    /// <param name="left">Left.</param>
    /// <param name="right">Rigth.</param>
    /// <returns>bool.</returns>
    public static bool operator <=(EnumerationBase? left, EnumerationBase? right)
    => left is null || left.CompareTo(right) <= 0;

    /// <summary>
    /// operator.
    /// </summary>
    /// <param name="left">Left.</param>
    /// <param name="right">Rigth.</param>
    /// <returns>bool.</returns>
    public static bool operator >(EnumerationBase? left, EnumerationBase? right)
    => left is not null && left.CompareTo(right) > 0;

    /// <summary>
    /// operator.
    /// </summary>
    /// <param name="left">Left.</param>
    /// <param name="right">Rigth.</param>
    /// <returns>bool.</returns>
    public static bool operator >=(EnumerationBase? left, EnumerationBase? right)
    => left is null ? right is null : left.CompareTo(right) >= 0;

    #endregion Operator

    /// <summary>
    /// Obtiene una enumeración por el id.
    /// </summary>
    /// <typeparam name="T">De tipo EnumerationBase.</typeparam>
    /// <param name="id">Id de la enumeración.</param>
    /// <returns>Enumeración | null.</returns>
    public static T? ToEnum<T>(int id)
        where T : EnumerationBase => ToAll<T>().FirstOrDefault(x => x.Id == id);

    /// <summary>
    /// Obtiene una enumeración por el nombre o el nombre corto.
    /// </summary>
    /// <typeparam name="T">De tipo EnumerationBase.</typeparam>
    /// <param name="nameOrShortName">Nombre o nombre corto de la enumeración.</param>
    /// <returns>Enumeración | null.</returns>
    public static T? ToEnum<T>(string? nameOrShortName)
        where T : EnumerationBase => ToAll<T>().FirstOrDefault(x => x.Name == nameOrShortName || x.ShortName == nameOrShortName);

    /// <summary>
    /// Obtiene una enumeración por el nombre o el nombre corto.
    /// </summary>
    /// <typeparam name="T">De tipo EnumerationBase.</typeparam>
    /// <param name="enumerationBase">Enumeración.</param>
    /// <returns>Enumeración | null.</returns>
    public static T? ToEnum<T>(EnumerationBase enumerationBase)
        where T : EnumerationBase => ToAll<T>().FirstOrDefault(x => x.Id == enumerationBase.Id || x.Name == enumerationBase.Name || x.ShortName == enumerationBase.ShortName);

    /// <summary>
    /// Obtiene la lista de una enumeración.
    /// </summary>
    /// <returns>IEnumerable{T}.</returns>
    public static IEnumerable<T> ToAll<T>()
        where T : EnumerationBase =>
        typeof(T).GetFields(BindingFlags.Public |
                            BindingFlags.Static |
                            BindingFlags.DeclaredOnly)
                 .Select(f => f.GetValue(null))
                 .Cast<T>();

    /// <summary>
    /// Obtiene un string con el nombre de la enumeración.
    /// </summary>
    /// <returns>{Name}.</returns>
    public override string ToString() => Name;

    /// <summary>
    /// Obtiene un string con el id y el nombre de la enumeración.
    /// </summary>
    /// <returns>{Id}: {Name}.</returns>
    public string ToFullString() => $"{Id}: {Name}";

    /// <summary>
    /// Compara dos objetos.
    /// </summary>
    /// <param name="obj">Objeto a comparar.</param>
    /// <returns>Verdadero si son del mismo tipo y tienen el mismo valor del id, falso si no se cumplen alguna de las 2 promesas anteriores.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not EnumerationBase otherValue)
        {
            return false;
        }

        var typeMatches = GetType().Equals(obj?.GetType());
        var valueMatches = Id.Equals(otherValue.Id);

        return typeMatches && valueMatches;
    }

    /// <summary>
    /// Compara este objeto con otro objeto y devuelve un número entero que indica la relación.
    /// </summary>
    /// <param name="obj">Valor a comparar.</param>
    /// <returns>
    ///     -1: si Id es menor que other.Id
    ///      0: si Id es igual que other.Id
    ///      1: si Id es mayor que other.Id
    ///      1: si other es null (nulo se considera menor que cualquier instancia, por lo tanto, devuelve un número positivo)
    /// Si el objeto no es del tipo Int32, este método lanza una ArgumentException.
    /// </returns>
    public int CompareTo(object? obj)
        => obj is null ? 1 : Id.CompareTo(((EnumerationBase)obj).Id);

    /// <summary>
    /// // El valor absoluto del id:int contenido.
    /// </summary>
    /// <returns>int.</returns>
    public override int GetHashCode() => Id.GetHashCode();
}
