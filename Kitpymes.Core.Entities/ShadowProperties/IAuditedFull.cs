// -----------------------------------------------------------------------
// <copyright file="IAuditedFull.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.Entities.ShadowProperties;

/// <summary>
/// Generar campos con la fecha de creación y modificación y eliminación de un registro.
/// </summary>
public interface IAuditedFull : IAuditedCreation, IAuditedModification, IAuditedDeletion { }
