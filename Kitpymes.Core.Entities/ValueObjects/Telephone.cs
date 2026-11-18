// -----------------------------------------------------------------------
// <copyright file="Telephone.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.Entities.ValueObjects;

using System.Diagnostics.CodeAnalysis;
using Kitpymes.Core.Shared.Extensions;

/// <summary>
/// Objeto de valor para teléfonos.
/// </summary>
public sealed class Telephone : ValueObjectBase
{
    private Telephone() { }

    private Telephone(string? prefix, long number)
    => ChangePrefix(prefix).ChangeNumber(number);

    /// <summary>
    /// Obtiene un teléfono vacio.
    /// </summary>
    public static Telephone Null => new ();

    /// <summary>
    /// Obtiene un valor que indica si el teléfono esta vacio.
    /// </summary>
    public bool IsNull => Prefix.ToIsNullOrEmpty() || Number.ToIsNullOrEmpty();

    /// <summary>
    /// Obtiene el prefijo del pais.
    /// </summary>
    public string? Prefix { get; private set; }

    /// <summary>
    /// Obtiene el número.
    /// </summary>
    public long? Number { get; private set; }

    /// <summary>
    /// Crea un nuevo télefono.
    /// </summary>
    /// <param name="prefix">Prefijo del pais.</param>
    /// <param name="number">Número de télefono.</param>
    /// <returns>Telephone | SharedException.</returns>
    [return: NotNull]
    public static Telephone Create(string? prefix, long number)
    => new (prefix, number);

    /// <summary>
    /// Modifica el prefijo del pais.
    /// </summary>
    /// <param name="prefix">Prefijo del pais.</param>
    /// <returns>Telephone.</returns>
    [return: NotNull]
    public Telephone ChangePrefix(string? prefix)
    {
        Prefix = prefix.ToThrowIfNullOrEmpty();

        return this;
    }

    /// <summary>
    /// Modifica el número de télefono.
    /// </summary>
    /// <param name="number">Número de télefono.</param>
    /// <returns>Telephone.</returns>
    [return: NotNull]
    public Telephone ChangeNumber(long number)
    {
        Number = number.ToThrowIfLess(1);

        return this;
    }

    /// <summary>
    /// Devuelve el télefono completo.
    /// </summary>
    /// <returns>"{Prefix} {Number}".</returns>
    public override string ToString() => $"{Prefix} {Number}";

    /// <inheritdoc/>
    protected override System.Collections.Generic.IEnumerable<object?> GetEqualityComponents()
    {
        yield return Prefix;
        yield return Number;
    }
}
