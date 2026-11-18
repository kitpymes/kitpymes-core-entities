// -----------------------------------------------------------------------
// <copyright file="EntityBaseGuid.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.Entities;

using System;

/// <summary>
/// Entidad base para is de tipo Guid.
/// </summary>
public abstract class EntityBaseGuid : EntityBase<Guid>
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="EntityBaseGuid"/>.
    /// Crea una clave automática.
    /// </summary>
    protected EntityBaseGuid()
        : base(Guid.NewGuid()) { }
}
