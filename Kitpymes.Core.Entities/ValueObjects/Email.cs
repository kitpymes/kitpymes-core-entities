// -----------------------------------------------------------------------
// <copyright file="Email.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.Entities.ValueObjects;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Kitpymes.Core.Entities.Internal;

/// <summary>
/// Objeto de valor para los emails.
/// </summary>
public sealed class Email : ValueObjectBase
{
    private Email() { }

    private Email(string? email) => Change(email);

    /// <summary>
    /// Obtiene un correo electronico vacio.
    /// </summary>
    public static Email Null => new ();

    /// <summary>
    /// Obtiene un valor que indica si el correo electronico esta vacio.
    /// </summary>
    public bool IsNull => Value.ToIsNullOrEmpty();

    /// <summary>
    /// Obtiene el correo electronico.
    /// </summary>
    public string? Value { get; private set; }

    /// <summary>
    /// Crea un nuevo correo electronico.
    /// </summary>
    /// <param name="email">Correo electronico.</param>
    /// <returns>Email | ArgumentException.</returns>
    [return: NotNull]
    public static Email Create(string? email) => new (email);

    /// <summary>
    /// Modifica el correo elecronico.
    /// </summary>
    /// <param name="email">Nuevo correo elecronico.</param>
    public void Change(string? email) => Value = email.ToThrowIfNotEmail();

    /// <summary>
    /// Devuelve el email.
    /// </summary>
    /// <returns>string.</returns>
    public override string ToString() => $"{Value}";

    /// <summary>
    /// Normaliza el email.
    /// </summary>
    /// <returns>string | null.</returns>
    public string? ToNormalize() => Value.ToReplaceSpecialChars(keepAt: true);

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
