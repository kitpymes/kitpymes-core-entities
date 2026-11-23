// -----------------------------------------------------------------------
// <copyright file="StatusEnum.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.Entities.Enumerations;

using System;
using Kitpymes.Core.Entities.Internal;

/*
   Clase de enumeraciones de estado StatusEnum
   Contiene la lista de estados
*/

/// <summary>
/// Clase de enumeraciones de estado <c>StatusEnum</c>.
/// Contiene la lista de estados.
/// <list type="bullet">
///     <item>
///         <term>Inactive = 0</term>
///         <description>Estado inactivo</description>
///     </item>
///     <item>
///         <term>Active = 1</term>
///         <description>Estado activo</description>
///     </item>
/// </list>
/// </summary>
/// <remarks>
/// <para>En esta clase se pueden agregar todos los tipo de estados.</para>
/// </remarks>
public class StatusEnum : EnumerationBase
{
    /// <summary>
    /// Estado inactivo.
    /// <para><strong>Inactive</strong> = 0.</para>
    /// </summary>
    public static readonly StatusEnum Inactive = new (0, nameof(Inactive));

    /// <summary>
    /// Estado activo.
    /// <para><strong>Active</strong> = 1.</para>
    /// </summary>
    public static readonly StatusEnum Active = new (1, nameof(Active));

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="StatusEnum"/>.
    /// </summary>
    /// <param name="id">Id del estado.</param>
    /// <param name="name">Nombre del estado.</param>
    public StatusEnum(int id, string name)
        : base(id, name)
    {
        id.ToThrowIfOutRange(0, 1);
    }

    /// <summary>
    /// Obtiene un valor que indica si el estado es activo.
    /// </summary>
    public bool IsActive => Name == Active.Name;

    /// <summary>
    /// Obtiene un valor que indica si el estado es inactivo.
    /// </summary>
    public bool IsInactive => Name == Inactive.Name;

    /// <summary>
    /// Convierte un booleano en una enumeración.
    /// </summary>
    /// <param name="status">Booleano a convertir.</param>
    /// <returns>Status.</returns>
    public static StatusEnum ToEnum(bool status) => status ? Active : Inactive;

    /// <summary>
    /// Convierte un string en una enumeración.
    /// </summary>
    /// <param name="status">String a convertir.</param>
    /// <returns>Status.</returns>
    public static StatusEnum ToEnum(string? status)
    {
        if (status is null || (status != Active.ToString() && status != Inactive.ToString()))
        {
            throw new ArgumentException($"{nameof(status)} not found.", nameof(status));
        }

        return status == Active.ToString() ? Active : Inactive;
    }

    /// <summary>
    /// Convierte un estado en booleano.
    /// </summary>
    /// <param name="status">Nombre del estado.</param>
    /// <returns>true | false | ArgumentException.</returns>
    public static bool ToBool(StatusEnum status) => status.ToThrowIfNullOrEmpty().Name == Active.ToString();

    /// <summary>
    /// Convierte un estado en booleano.
    /// </summary>
    /// <param name="status">Nombre del estado.</param>
    /// <returns>true | false | ArgumentException.</returns>
    public static bool ToBool(string? status)
    {
        if (status is null || (status != Active.ToString() && status != Inactive.ToString()))
        {
            throw new ArgumentException($"{nameof(status)} not found.", nameof(status));
        }

        return status == Active.ToString();
    }
}
