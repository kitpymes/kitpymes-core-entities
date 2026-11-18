// -----------------------------------------------------------------------
// <copyright file="EntityBaseString.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.Entities;

using System;

/// <summary>
/// Entidad base para is de tipo string.
/// </summary>
public abstract class EntityBaseString : EntityBase<string>
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="EntityBaseString"/>.
    /// Crea una clave automática.
    /// </summary>
    protected EntityBaseString()
        : base(Guid.NewGuid().ToString()) { }
}
