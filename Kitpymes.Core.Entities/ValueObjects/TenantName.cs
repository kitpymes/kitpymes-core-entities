// -----------------------------------------------------------------------
// <copyright file="TenantName.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.Entities.ValueObjects;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Kitpymes.Core.Entities.Internal;

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
    /// <returns>Name | ArgumentException.</returns>
    [return: NotNull]
    public static TenantName Create(string? tenantName) => new (tenantName);

    /// <summary>
    /// Modifica el nombre.
    /// </summary>
    /// <param name="tenantName">Nombre nuevo a cambiar.</param>
    /// <exception cref="ArgumentNullException">Si el nombre es nulo.</exception>
    /// <exception cref="ArgumentException">Si está vacío o tiene caracteres que no son letras sin acento, números o guion bajo.</exception>
    public void Change(string? tenantName)
    {
        const string message = "Only numbers, letters and underscore are allowed";

        if (tenantName is null)
        {
            throw new ArgumentNullException(nameof(tenantName), message);
        }

        Value = tenantName.ToIsRegexMatch(@"^[_a-zA-Z0-9]*$") ? tenantName : throw new ArgumentException(message, nameof(tenantName));
    }

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
