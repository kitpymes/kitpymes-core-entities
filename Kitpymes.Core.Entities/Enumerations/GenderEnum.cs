// -----------------------------------------------------------------------
// <copyright file="GenderEnum.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.Entities.Enumerations;

using Kitpymes.Core.Entities.Internal;

/*
   Clase de enumeraciones para los tipo de sexo GenderEnum
   Contiene la lista de los tipo de sexo
*/

/// <summary>
/// Clase de enumeraciones para los tipo de sexo <c>GenderEnum</c>.
/// Contiene la lista de los tipo de sexo.
/// <list type="bullet">
///     <item>
///         <term>None = 0</term>
///         <description>No tiene sexo</description>
///     </item>
///     <item>
///         <term>Male = 1</term>
///         <description>Sexo masculino</description>
///     </item>
///     <item>
///         <term>Female = 2</term>
///         <description>Sexo femenino</description>
///     </item>
/// </list>
/// </summary>
/// <remarks>
/// <para>En esta clase se pueden agregar todos los tipo de sexo.</para>
/// </remarks>
#pragma warning disable CA1711 // Los identificadores no deben tener un sufijo incorrecto
public class GenderEnum : EnumerationBase
#pragma warning restore CA1711 // Los identificadores no deben tener un sufijo incorrecto
{
    /// <summary>
    /// No tiene.
    /// <para><strong>Sin sexo</strong> = 0.</para>
    /// </summary>
    public static readonly GenderEnum None = new (0, nameof(None), nameof(None));

    /// <summary>
    /// Sexo masculino.
    /// <para><strong>Male</strong> = 1.</para>
    /// </summary>
    public static readonly GenderEnum Male = new (1, nameof(Male), "M");

    /// <summary>
    /// Sexo femenino.
    /// <para><strong>Female</strong> = 2.</para>
    /// </summary>
    public static readonly GenderEnum Female = new (2, nameof(Female), "F");

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="GenderEnum"/>.
    /// </summary>
    /// <param name="id">Id del tipo de sexo.</param>
    /// <param name="name">Nombre del sexo.</param>
    /// <param name="shortName">Nombre corto del sexo.</param>
    public GenderEnum(int id, string name, string shortName)
        : base(id, name, shortName)
    {
        id.ToThrowIfOutRange(0, 2);
    }

    /// <summary>
    /// Obtiene un valor que indica si tiene sexo.
    /// </summary>
    public bool IsNone => Name == None.Name;

    /// <summary>
    /// Obtiene un valor que indica si el sexo es masculino.
    /// </summary>
    public bool IsMale => Name == Male.Name;

    /// <summary>
    /// Obtiene un valor que indica si el sexo es femenino.
    /// </summary>
    public bool IsFemale => Name == Female.Name;
}
