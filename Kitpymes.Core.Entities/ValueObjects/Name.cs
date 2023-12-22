// -----------------------------------------------------------------------
// <copyright file="Name.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.Entities.ValueObjects;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Kitpymes.Core.Shared.Extensions;

/// <summary>
/// Objeto de valor para nombres.
/// </summary>
public sealed class Name : ValueObjectBase
{
    private Name() { }

    private Name(string? name) => Change(name);

    /// <summary>
    /// Obtiene un nombre vacio.
    /// </summary>
    public static Name Null => new ();

    /// <summary>
    /// Obtiene un valor que indica si el nombre esta vacio.
    /// </summary>
    public bool IsNull => Value.ToIsNullOrEmpty();

    /// <summary>
    /// Obtiene el nombre.
    /// </summary>
    public string? Value { get; private set; }

    /// <summary>
    /// Crea un nuevo nombre.
    /// </summary>
    /// <param name="name">Nombre nuevo.</param>
    /// <returns>Name | SharedException.</returns>
    [return: NotNull]
    public static Name Create(string? name) => new (name);

    /// <summary>
    /// Modifica el nombre.
    /// </summary>
    /// <param name="name">Nombre nuevo.</param>
    public void Change(string? name) => Value = name.ToThrowIfNotName();

    /// <summary>
    /// Devuelve el nombre.
    /// </summary>
    /// <returns>string.</returns>
    public override string ToString() => $"{Value}";

    /// <summary>
    /// Normaliza el nombre.
    /// </summary>
    /// <returns>string | null.</returns>
    public string? ToNormalize() => Value.ToNormalize();

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
