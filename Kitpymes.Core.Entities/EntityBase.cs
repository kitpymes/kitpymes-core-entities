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
using System.Runtime.CompilerServices;
using Kitpymes.Core.Entities.Internal;
using Kitpymes.Core.Mediator.Abstractions;

/// <summary>
/// Entidad base: identidad por clave y eventos de dominio.
/// </summary>
/// <remarks>
/// <para>Dos entidades son iguales si son del mismo tipo y tienen la misma clave. Una entidad sin clave
/// (con el valor por defecto de <typeparamref name="TKey"/>, p. ej. todavía sin guardar) sólo es igual a sí misma.</para>
/// <para>El código hash cambia cuando una entidad sin clave recibe la suya: no la guarde en un <see cref="HashSet{T}"/>
/// o como clave de un diccionario antes de asignarle la clave.</para>
/// </remarks>
/// <typeparam name="TKey">Tipo de la clave.</typeparam>
public abstract class EntityBase<TKey> : IEquatable<EntityBase<TKey>>, IEntityBase
{
    private readonly List<INotification> _domainEvents = [];

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="EntityBase{TKey}"/> sin clave.
    /// </summary>
#pragma warning disable CS8618 // La clave la asigna después la base de datos o el ORM.
    protected EntityBase() { }
#pragma warning restore CS8618

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="EntityBase{TKey}"/>.
    /// </summary>
    /// <param name="key">Clave única de la entidad.</param>
    /// <exception cref="ArgumentNullException">Si la clave es nula.</exception>
    /// <exception cref="ArgumentException">Si la clave es una cadena vacía o <see cref="Guid.Empty"/>.</exception>
    protected EntityBase(TKey key) => Id = key.ToThrowIfNullOrEmpty();

    /// <summary>
    /// Obtiene o establece la clave de la entidad.
    /// </summary>
    public virtual TKey Id { get; protected set; }

    /// <inheritdoc/>
    [NotMapped]
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Verifica que dos entidades son iguales.
    /// </summary>
    /// <param name="left">Entidad de la izquierda.</param>
    /// <param name="right">Entidad de la derecha.</param>
    /// <returns>true | false.</returns>
    public static bool operator ==(EntityBase<TKey>? left, EntityBase<TKey>? right)
        => left is null ? right is null : left.Equals(right);

    /// <summary>
    /// Verifica que dos entidades no son iguales.
    /// </summary>
    /// <param name="left">Entidad de la izquierda.</param>
    /// <param name="right">Entidad de la derecha.</param>
    /// <returns>true | false.</returns>
    public static bool operator !=(EntityBase<TKey>? left, EntityBase<TKey>? right) => !(left == right);

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">Si el evento es nulo.</exception>
    public void AddDomainEvent(INotification domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        _domainEvents.Add(domainEvent);
    }

    /// <inheritdoc/>
    public void RemoveDomainEvent(INotification domainEvent) => _domainEvents.Remove(domainEvent);

    /// <inheritdoc/>
    public void ClearDomainEvents() => _domainEvents.Clear();

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as EntityBase<TKey>);

    /// <inheritdoc/>
    public bool Equals(EntityBase<TKey>? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (GetType() != other.GetType() || IsTransient() || other.IsTransient())
        {
            return false;
        }

        return EqualityComparer<TKey>.Default.Equals(Id, other.Id);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
        => IsTransient() ? RuntimeHelpers.GetHashCode(this) : HashCode.Combine(GetType(), Id);

    private bool IsTransient() => Id is null || EqualityComparer<TKey>.Default.Equals(Id, default);
}
