// -----------------------------------------------------------------------
// <copyright file="SubscriptionEnum.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.Entities.Enumerations;

using Kitpymes.Core.Entities.Internal;

/*
   Clase de enumeraciones de subscripciones SubscriptionEnum
   Contiene la lista de subscripciones
*/

/// <summary>
/// Clase de enumeraciones de subscripciones <c>SubscriptionEnum</c>.
/// Contiene la lista de subscripcione.
/// <list type="bullet">
///     <item>
///         <term>None = 0</term>
///         <description>No tiene subscripción</description>
///     </item>
///     <item>
///         <term>Free = 1</term>
///         <description>Subscripción gratis</description>
///     </item>
///     <item>
///         <term>Silver = 2</term>
///         <description>Subscripción plata</description>
///     </item>
///     <item>
///         <term>Gold = 3</term>
///         <description>Subscripción oro</description>
///     </item>
/// </list>
/// </summary>
/// <remarks>
/// <para>En esta clase se pueden agregar todas las subscripciones.</para>
/// </remarks>
public class SubscriptionEnum : EnumerationBase
{
    /// <summary>
    /// No tiene Subscripción.
    /// <para><strong>None</strong> = 0.</para>
    /// </summary>
    public static readonly SubscriptionEnum None = new (0, nameof(None), nameof(None));

    /// <summary>
    /// Subscripción gratis.
    /// <para><strong>Free</strong> = 1.</para>
    /// </summary>
    public static readonly SubscriptionEnum Free = new (1, nameof(Free), nameof(Free));

    /// <summary>
    /// Subscripción plata.
    /// <para><strong>Silver</strong> = 2.</para>
    /// </summary>
    public static readonly SubscriptionEnum Silver = new (2, nameof(Silver), nameof(Silver));

    /// <summary>
    /// Subscripción oro.
    /// <para><strong>Gold</strong> = 3.</para>
    /// </summary>
    public static readonly SubscriptionEnum Gold = new (3, nameof(Gold), nameof(Gold));

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SubscriptionEnum"/>.
    /// </summary>
    /// <param name="id">Id de la subscripción.</param>
    /// <param name="name">Nombre de la subscripción.</param>
    /// <param name="shortName">Nombre corto de la subscripción.</param>
    public SubscriptionEnum(int id, string name, string shortName)
        : base(id, name, shortName)
    {
        id.ToThrowIfOutRange(0, 3);
    }

    /// <summary>
    /// Obtiene un valor que indica si tiene una subscripción.
    /// </summary>
    public bool IsNone => Name == None.Name;

    /// <summary>
    /// Obtiene un valor que indica si la subscripción es gratis.
    /// </summary>
    public bool IsFree => Name == Free.Name;

    /// <summary>
    /// Obtiene un valor que indica si la subscripción es plata.
    /// </summary>
    public bool IsSilver => Name == Silver.Name;

    /// <summary>
    /// Obtiene un valor que indica si la subscripción es gold.
    /// </summary>
    public bool IsGold => Name == Gold.Name;
}