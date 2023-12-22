// -----------------------------------------------------------------------
// <copyright file="Subdomain.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.Entities.ValueObjects;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Kitpymes.Core.Shared.Extensions;

/// <summary>
/// Objeto de valor para subdominios.
/// </summary>
public sealed class Subdomain : ValueObjectBase
{
    private Subdomain() { }

    private Subdomain(string? subdomain) => Change(subdomain);

    /// <summary>
    /// Obtiene un subdominio vacio.
    /// </summary>
    public static Subdomain Null => new ();

    /// <summary>
    /// Obtiene un valor que indica si el subdominio esta vacio.
    /// </summary>
    public bool IsNull => Value.ToIsNullOrEmpty();

    /// <summary>
    /// Obtiene el subdominio.
    /// </summary>
    public string? Value { get; private set; }

    /// <summary>
    /// Crea un nuevo nombre.
    /// </summary>
    /// <param name="subdomain">Subdominio nuevo.</param>
    /// <returns>Subdomain | SharedException.</returns>
    [return: NotNull]
    public static Subdomain Create(string? subdomain) => new (subdomain.ToThrowIfNotSubdomain().ToLower());

    /// <summary>
    /// Modifica un subdominio.
    /// </summary>
    /// <param name="subdomain">Subdominio nuevo.</param>
    public void Change(string? subdomain) => Value = subdomain.ToThrowIfNotSubdomain();

    /// <summary>
    /// Devuelve el subdomain.
    /// </summary>
    /// <returns>string.</returns>
    public override string ToString() => $"{Value}";

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
