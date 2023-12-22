// -----------------------------------------------------------------------
// <copyright file="LogEnum.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.Entities.Enumerations;

using Kitpymes.Core.Shared.Extensions;

/*
   Clase de enumeraciones para el log de las acciones de una entidad LogEnum
   Contiene la lista de las acciones de una entidad
*/

/// <summary>
/// Clase de enumeraciones para el log de las acciones de una entidad <c>LogEnum</c>.
/// Contiene la lista de las acciones de una entidad.
/// <list type="bullet">
///     <item>
///         <term>None = 0</term>
///         <description>No tiene log</description>
///     </item>
///     <item>
///         <term>None = 1</term>
///         <description>No hubo ningún cambio de una entidad</description>
///     </item>
///     <item>
///         <term>Created = 2</term>
///         <description>Creación de una entidad</description>
///     </item>
///     <item>
///         <term>Updated = 3</term>
///         <description>Actualización de una entidad</description>
///     </item>
///     <item>
///         <term>Deleted = 4</term>
///         <description>Eliminación de una entidad</description>
///     </item>
///     <item>
///         <term>Changed = 5</term>
///         <description>Cambio de estado de una entidad</description>
///     </item>
/// </list>
/// </summary>
/// <remarks>
/// <para>En esta clase se pueden agregar todos los tipo de log.</para>
/// </remarks>
public class LogEnum : EnumerationBase
{
    /// <summary>
    /// Sin cambios.
    /// <para><strong>None</strong> = 0.</para>
    /// </summary>
    public static readonly LogEnum None = new (0, nameof(None), nameof(None));

    /// <summary>
    /// Creación.
    /// <para><strong>Created</strong> = 1.</para>
    /// </summary>
    public static readonly LogEnum Created = new (1, nameof(Created), nameof(Created));

    /// <summary>
    /// Actualización.
    /// <para><strong>Updated</strong> = 2.</para>
    /// </summary>
    public static readonly LogEnum Updated = new (2, nameof(Updated), nameof(Updated));

    /// <summary>
    /// Eliminación.
    /// <para><strong>Deleted</strong> = 3.</para>
    /// </summary>
    public static readonly LogEnum Deleted = new (3, nameof(Deleted), nameof(Deleted));

    /// <summary>
    /// Cambio de estado.
    /// <para><strong>Changed</strong> = 4.</para>
    /// </summary>
    public static readonly LogEnum Changed = new (4, nameof(Changed), nameof(Changed));

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="LogEnum"/>.
    /// </summary>
    /// <param name="id">Id del log.</param>
    /// <param name="name">Nombre del log.</param>
    /// <param name="shortName">Nombre corto del log.</param>
    public LogEnum(int id, string name, string shortName)
        : base(id, name, shortName)
    {
        id.ToThrowIfOutRange(0, 4);
    }

    /// <summary>
    /// Obtiene un valor que indica si tiene un log.
    /// </summary>
    public bool IsNone => Name == None.Name;

    /// <summary>
    /// Obtiene un valor que indica si fue creado.
    /// </summary>
    public bool IsCreated => Name == Created.Name;

    /// <summary>
    /// Obtiene un valor que indica si fue actualizado.
    /// </summary>
    public bool IsUpdated => Name == Updated.Name;

    /// <summary>
    /// Obtiene un valor que indica si fue eliminado.
    /// </summary>
    public bool IsDeleted => Name == Deleted.Name;

    /// <summary>
    /// Obtiene un valor que indica si cambio su estado.
    /// </summary>
    public bool IsChanged => Name == Changed.Name;
}
