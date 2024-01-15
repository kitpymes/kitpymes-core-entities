// -----------------------------------------------------------------------
// <copyright file="IEntityBase.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.Entities;

using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Interfaz base para las entidades.
/// </summary>
public interface IEntityBase
{
    /// <summary>
    /// Obtiene eventos de dominio.
    /// </summary>
    [NotMapped]
    public IReadOnlyCollection<INotification> DomainEvents { get; }

    /// <summary>
    /// Agrega un evento de dominio.
    /// </summary>
    /// <param name="domainEvent">Evento de dominio.</param>
    public void AddDomainEvent(INotification domainEvent);

    /// <summary>
    /// Elimina un evento de dominio.
    /// </summary>
    /// <param name="domainEvent">Evento de dominio.</param>
    public void RemoveDomainEvent(INotification domainEvent);

    /// <summary>
    /// Elimina todos los eventos de dominio.
    /// </summary>
    public void ClearDomainEvents();
}