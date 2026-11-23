// -----------------------------------------------------------------------
// <copyright file="Money.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.Entities.ValueObjects;

using Kitpymes.Core.Entities.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

/// <summary>
/// Objeto de valor para el dinero.
/// </summary>
public sealed class Money : ValueObjectBase
{
    private const decimal DefaultAmount = 0;
    private const int DefaultNumbeOfDecimals = 2;

    // Para el ORM, que rellena las propiedades después.
    private Money() { }

    private Money(decimal amount, Currency currency, int numbeOfDecimals)
    => ChangeNumbeOfDecimals(numbeOfDecimals).ChangeAmount(amount).ChangeCurrency(currency);

    /// <summary>
    /// Obtiene un nuevo dinero con valores por defecto.
    /// </summary>
    /// <exception cref="InvalidOperationException">Si no se puede deducir la moneda de la cultura actual (ver <see cref="Currency.Default"/>).</exception>
    public static Money Default => new (DefaultAmount, Currency.Default, DefaultNumbeOfDecimals);

    /// <summary>
    /// Obtiene la cantidad de dinero.
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Obtiene el simbolo del tipo de moneda.
    /// </summary>
    public Currency? Currency { get; private set; }

    /// <summary>
    /// Obtiene la cantidad de decimales permitidos.
    /// </summary>
    public int? NumbeOfDecimals { get; private set; }

    /// <summary>
    /// Crea un dinero.
    /// </summary>
    /// <param name="amount">Cantidad de dinero.</param>
    /// <param name="currency">Tipo de moneda.</param>
    /// <param name="numbeOfDecimals">Cantidad de decimales permitidos.</param>
    /// <returns>Money | ArgumentException.</returns>
    [return: NotNull]
    public static Money Create(decimal amount, Currency currency, int numbeOfDecimals = DefaultNumbeOfDecimals)
    => new (amount, currency, numbeOfDecimals);

    /// <summary>
    /// Modifica la cantidad de dinero.
    /// </summary>
    /// <param name="amount">Cantidad de dinero.</param>
    /// <returns>Money.</returns>
    [return: NotNull]
    public Money ChangeAmount(decimal amount)
    {
        if (Amount != amount)
        {
            Amount = Math.Round(amount, NumbeOfDecimals ?? DefaultNumbeOfDecimals);
        }

        return this;
    }

    /// <summary>
    /// Modifica el tipo de moneda.
    /// </summary>
    /// <param name="currency">Tipo de moneda.</param>
    /// <returns>Money.</returns>
    [return: NotNull]
    public Money ChangeCurrency(Currency currency)
    {
        Currency = currency;

        return this;
    }

    /// <summary>
    /// Modifica la cantidad de decimales permitidos.
    /// </summary>
    /// <param name="numbeOfDecimals">Número de decimales permitidos.</param>
    /// <returns>Money.</returns>
    [return: NotNull]
    public Money ChangeNumbeOfDecimals(int numbeOfDecimals)
    {
        if (NumbeOfDecimals != numbeOfDecimals.ToThrowIfLess(0))
        {
            NumbeOfDecimals = numbeOfDecimals;
        }

        return this;
    }

    /// <summary>
    /// Devuelve la cantidad de dinero.
    /// </summary>
    /// <returns>"{Currency} {Amount}".</returns>
    public override string ToString() => $"{Currency?.ToString()} {Amount.ToString(CultureInfo.CurrentCulture)}";

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Currency?.ToString();
        yield return Amount;
    }
}
