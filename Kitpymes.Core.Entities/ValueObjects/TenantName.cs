// -----------------------------------------------------------------------
// <copyright file="TenantName.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.Entities.ValueObjects;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Kitpymes.Core.Shared.Extensions;

/// <summary>
/// Objeto de valor para nombres de tenants.
/// </summary>
public sealed class TenantName : ValueObjectBase
{
    private TenantName() { }

    private TenantName(string? tenantName) => Change(tenantName);

    /// <summary>
    /// Obtiene un nombre vacio.
    /// </summary>
    public static TenantName Null => new ();

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
    /// <param name="tenantName">Nombre nuevo.</param>
    /// <returns>Name | SharedException.</returns>
    [return: NotNull]
    public static TenantName Create(string? tenantName) => new (tenantName);

    /// <summary>
    /// Modifica el nombre.
    /// </summary>
    /// <param name="tenantName">Nombre nuevo a cambiar.</param>
    public void Change(string? tenantName) => Value = tenantName.ToThrowIf("Only numbers, letters and underscore are allowed", () => !tenantName.ToIsRegexMatch(@"^[_a-zA-Z0-9]*$"));

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
