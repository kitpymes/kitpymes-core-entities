// -----------------------------------------------------------------------
// <copyright file="IEntityBase.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.Entities;

using System.Collections.Generic;
using Kitpymes.Core.Mediator.Abstractions;

/// <summary>
/// Interfaz base para las entidades con eventos de dominio.
/// </summary>
/// <remarks>
/// Los eventos son notificaciones de Kitpymes.Core.Mediator.Abstractions: se publican uno a uno, en el orden de
/// <see cref="DomainEvents"/>, con <see cref="IPublisher.PublishAsync"/>, que entrega cada evento a sus manejadores
/// en orden y para en el primero que falla.
/// </remarks>
public interface IEntityBase
{
    /// <summary>
    /// Obtiene los eventos de dominio pendientes de publicar, en el orden en que se añadieron.
    /// </summary>
    IReadOnlyCollection<INotification> DomainEvents { get; }

    /// <summary>
    /// Agrega un evento de dominio.
    /// </summary>
    /// <param name="domainEvent">Evento de dominio.</param>
    void AddDomainEvent(INotification domainEvent);

    /// <summary>
    /// Elimina un evento de dominio.
    /// </summary>
    /// <param name="domainEvent">Evento de dominio.</param>
    void RemoveDomainEvent(INotification domainEvent);

    /// <summary>
    /// Elimina todos los eventos de dominio.
    /// </summary>
    void ClearDomainEvents();
}
