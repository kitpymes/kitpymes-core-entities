// -----------------------------------------------------------------------
// <copyright file="EntityBase.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Kitpymes.Core.Shared.Extensions;
using Kitpymes.Core.Shared.Util;
using MediatR;

/// <summary>
/// Entidad base.
/// </summary>
/// <typeparam name="TKey">Entity key type.</typeparam>
public abstract class EntityBase<TKey> : IEquatable<TKey>, IEntityBase
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="EntityBase{TKey}"/>.
    /// </summary>
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    protected EntityBase() { }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="EntityBase{TKey}"/>.
    /// </summary>
    /// <param name="key">Clave única de una entidad.</param>
    protected EntityBase(TKey key) => Id = key.ToThrowIfNullOrEmpty();

    /// <summary>
    /// Obtiene o establece la clave de la entidad.
    /// </summary>
    public virtual TKey Id { get; protected set; }

    #region DomainEvents

#pragma warning disable SA1201 // Elements should appear in the correct order
    private readonly List<INotification> _domainEvents = [];
#pragma warning restore SA1201 // Elements should appear in the correct order

    /// <summary>
    /// Obtiene eventos de dominio.
    /// </summary>
    [NotMapped]
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Agrega un evento de dominio.
    /// </summary>
    /// <param name="domainEvent">Evento de dominio.</param>
    protected void AddDomainEvent(INotification domainEvent) => _domainEvents.Add(domainEvent);

    /// <summary>
    /// Elimina un evento de dominio.
    /// </summary>
    /// <param name="domainEvent">Evento de dominio.</param>
    protected void RemoveDomainEvent(INotification domainEvent) => _domainEvents.Remove(domainEvent);

    /// <summary>
    /// Elimina todos los eventos de dominio.
    /// </summary>
    protected void ClearDomainEvents() => _domainEvents.Clear();

    #endregion DomainEvents

    #region Equals

    /// <summary>
    /// Verifica que dos objetos son iguales.
    /// </summary>
    /// <param name="left">Objeto fuente.</param>
    /// <param name="right">Objeto destino.</param>
    /// <returns>true | false.</returns>
#pragma warning disable SA1201 // Elements should appear in the correct order
    public static bool operator ==(EntityBase<TKey> left, EntityBase<TKey> right)
    {
        if (left is null && right is null)
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Equals(right);
    }
#pragma warning restore SA1201 // Elements should appear in the correct order

    /// <summary>
    /// Verifica que dos objetos no son iguales.
    /// </summary>
    /// <param name="left">Objeto fuente.</param>
    /// <param name="right">Objeto destino.</param>
    /// <returns>true | false.</returns>
    public static bool operator !=(EntityBase<TKey> left, EntityBase<TKey> right) => !(left == right);

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        Verify.ToThrowIf($"Invalid comparison of entities of different types: {GetType()} and {obj.GetType()}", () => GetType() != obj.GetType());

        return obj is EntityBase<TKey> entity && Equals(entity.Id);
    }

    /// <inheritdoc/>
    public bool Equals([AllowNull] TKey other) => Id?.Equals(other) ?? false;

    /// <inheritdoc/>
    public override int GetHashCode() => (GetType().Name + Id).GetHashCode(StringComparison.OrdinalIgnoreCase);

    #endregion Equals
}
