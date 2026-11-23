// -----------------------------------------------------------------------
// <copyright file="InternalExtensions.cs" company="Kitpymes">
// Copyright (c) Kitpymes. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project docs folder for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace Kitpymes.Core.Entities.Internal;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Buffers.Binary;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;

/*
    Lo que Entities usaba de Kitpymes.Core.Shared 2.0.1, copiado como interno para no depender de Shared
    (y, con él, del framework de ASP.NET Core). Mismas reglas y mismos mensajes que Shared 2.0.1.
    Diferencias:
    - Donde Shared lanzaba SharedException, aquí se lanzan las excepciones de .NET: ArgumentNullException si el
      valor es nulo, ArgumentOutOfRangeException si está fuera de rango y ArgumentException en el resto.
      Todas heredan de ArgumentException y llevan el nombre del argumento.
    - Los mensajes salen siempre en español: con nombre de campo, Shared 2.0.1 también los daba siempre en
      español, porque en su recurso inglés esas claves estaban mal escritas.
    - ToSecuencial guarda el contador entre llamadas: en Shared era una variable local y dos Guid creados
      en el mismo instante no quedaban ordenados.
*/

/// <summary>
/// Validaciones y utilidades internas de Entities.
/// </summary>
internal static class InternalExtensions
{
    private const string ForEmail = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

    private const string ForName = @"^[a-zA-Z áàäâãÁÀÄÂÃÅéèëêÉÈËÊíìïîÍÌÏÎóòöôõøÓÒÖÔÕúùüûÚÙÜÛçÇħñÑ]*$";

    private const string ForSubdomain = @"^[-._a-zA-Z0-9]*$";

    private static readonly TimeSpan RegexTimeout = TimeSpan.FromSeconds(1);

    private static readonly Dictionary<char, char[]> SpecialChars = new ()
    {
        { 'a', new[] { 'à', 'á', 'ä', 'â', 'ã' } },
        { 'A', new[] { 'À', 'Á', 'Ä', 'Â', 'Ã', 'Å' } },
        { 'c', new[] { 'ç' } },
        { 'C', new[] { 'Ç' } },
        { 'e', new[] { 'è', 'é', 'ë', 'ê' } },
        { 'E', new[] { 'È', 'É', 'Ë', 'Ê' } },
        { 'i', new[] { 'ì', 'í', 'ï', 'î' } },
        { 'I', new[] { 'Ì', 'Í', 'Ï', 'Î' } },
        { 'o', new[] { 'ò', 'ó', 'ö', 'ô', 'õ', 'ø' } },
        { 'O', new[] { 'Ò', 'Ó', 'Ö', 'Ô', 'Õ' } },
        { 'u', new[] { 'ù', 'ú', 'ü', 'û' } },
        { 'U', new[] { 'Ù', 'Ú', 'Ü', 'Û' } },
        { 'h', new[] { 'ħ' } },
        { 'n', new[] { 'ñ' } },
        { 'N', new[] { 'Ñ' } },
    };

    private static long sequentialCounter = DateTime.UtcNow.Ticks;

    /// <summary>
    /// Verifica si un argumento es nulo, una cadena vacía o con sólo espacios, o un <see cref="Guid.Empty"/>.
    /// </summary>
    /// <param name="argument">Argumento a verificar.</param>
    /// <returns>true | false.</returns>
    internal static bool ToIsNullOrEmpty([NotNullWhen(false)] this object? argument)
        => argument is null
            || (argument is string text && string.IsNullOrWhiteSpace(text))
            || (argument is Guid guid && guid == Guid.Empty);

    /// <summary>
    /// Lanza una excepción si el argumento es nulo o vacío (ver <see cref="ToIsNullOrEmpty"/>).
    /// </summary>
    /// <typeparam name="TArgument">Tipo del argumento.</typeparam>
    /// <param name="argument">Argumento a verificar.</param>
    /// <param name="argumentName">Nombre del argumento.</param>
    /// <returns>El argumento.</returns>
    /// <exception cref="ArgumentNullException">Si es nulo.</exception>
    /// <exception cref="ArgumentException">Si es vacío.</exception>
    internal static TArgument ToThrowIfNullOrEmpty<TArgument>([NotNull] this TArgument? argument, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
    {
        if (argument is null)
        {
            throw new ArgumentNullException(argumentName, Messages.Required(argumentName));
        }

        if (argument.ToIsNullOrEmpty())
        {
            throw new ArgumentException(Messages.Required(argumentName), argumentName);
        }

        return argument;
    }

    /// <summary>
    /// Lanza una excepción si el número es menor que el mínimo.
    /// </summary>
    /// <param name="argument">Número a verificar.</param>
    /// <param name="min">Mínimo permitido.</param>
    /// <param name="argumentName">Nombre del argumento.</param>
    /// <returns>El número.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Si es menor que el mínimo.</exception>
    internal static int ToThrowIfLess(this int argument, long min, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
    {
        ThrowIfLess(argument, min, argumentName);

        return argument;
    }

    /// <summary>
    /// Lanza una excepción si el número es menor que el mínimo.
    /// </summary>
    /// <param name="argument">Número a verificar.</param>
    /// <param name="min">Mínimo permitido.</param>
    /// <param name="argumentName">Nombre del argumento.</param>
    /// <returns>El número.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Si es menor que el mínimo.</exception>
    internal static long ToThrowIfLess(this long argument, long min, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
    {
        ThrowIfLess(argument, min, argumentName);

        return argument;
    }

    /// <summary>
    /// Lanza una excepción si el número es menor que el mínimo o mayor que el máximo.
    /// </summary>
    /// <param name="argument">Número a verificar.</param>
    /// <param name="min">Mínimo permitido.</param>
    /// <param name="max">Máximo permitido.</param>
    /// <param name="argumentName">Nombre del argumento.</param>
    /// <returns>El número.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Si está fuera del rango.</exception>
    internal static int ToThrowIfOutRange(this int argument, long min, long max, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
    {
        if (argument < min || argument > max)
        {
            throw new ArgumentOutOfRangeException(argumentName, Messages.OutRange(min, max, argumentName));
        }

        return argument;
    }

    /// <summary>
    /// Verifica si un argumento no vacío coincide con la expresión regular.
    /// </summary>
    /// <param name="argument">Argumento a verificar.</param>
    /// <param name="regex">Expresión regular.</param>
    /// <returns>true | false.</returns>
    /// <exception cref="RegexMatchTimeoutException">Si la evaluación supera el tiempo máximo (1 segundo).</exception>
    internal static bool ToIsRegexMatch([NotNullWhen(true)] this string? argument, string regex)
        => !argument.ToIsNullOrEmpty() && Regex.IsMatch(argument, regex, RegexOptions.CultureInvariant, RegexTimeout);

    /// <summary>
    /// Lanza una excepción si el argumento no es un nombre válido (letras, espacios y letras acentuadas).
    /// </summary>
    /// <param name="argument">Argumento a verificar.</param>
    /// <param name="argumentName">Nombre del argumento.</param>
    /// <returns>El argumento.</returns>
    internal static string ToThrowIfNotName([NotNull] this string? argument, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
        => argument.ToThrowIfNotMatch(ForName, argumentName);

    /// <summary>
    /// Lanza una excepción si el argumento no es un email válido.
    /// </summary>
    /// <param name="argument">Argumento a verificar.</param>
    /// <param name="argumentName">Nombre del argumento.</param>
    /// <returns>El argumento.</returns>
    internal static string ToThrowIfNotEmail([NotNull] this string? argument, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
        => argument.ToThrowIfNotMatch(ForEmail, argumentName);

    /// <summary>
    /// Lanza una excepción si el argumento no es un subdominio válido (letras, números, guion, punto y guion bajo).
    /// </summary>
    /// <param name="argument">Argumento a verificar.</param>
    /// <param name="argumentName">Nombre del argumento.</param>
    /// <returns>El argumento.</returns>
    internal static string ToThrowIfNotSubdomain([NotNull] this string? argument, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
        => argument.ToThrowIfNotMatch(ForSubdomain, argumentName);

    /// <summary>
    /// Sustituye las letras acentuadas por la letra sin acento y quita el resto de caracteres especiales.
    /// </summary>
    /// <remarks>Shared admitía cualquier lista de caracteres a conservar; Entities sólo conserva la arroba (Email).</remarks>
    /// <param name="input">Cadena de entrada.</param>
    /// <param name="keepAt">Si se conserva la arroba.</param>
    /// <returns>La cadena sin caracteres especiales.</returns>
    internal static string? ToReplaceSpecialChars(this string? input, bool keepAt = false)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return input;
        }

        input = SpecialChars.Keys.Aggregate(input, (x, y) => SpecialChars[y].Aggregate(x, (z, c) => z.Replace(c, y)));

        return Regex.Replace(input, keepAt ? "[^0-9a-zA-Z @]+?" : "[^0-9a-zA-Z ]+?", string.Empty, RegexOptions.None, RegexTimeout);
    }

    /// <summary>
    /// Normaliza una cadena: quita los caracteres especiales, sustituye las letras acentuadas y quita los espacios de los extremos.
    /// </summary>
    /// <param name="input">Cadena a normalizar.</param>
    /// <returns>Cadena normalizada.</returns>
    internal static string? ToNormalize(this string? input) => input.ToReplaceSpecialChars()?.Trim();

    /// <summary>
    /// Crea un Guid secuencial a partir de otro: sustituye sus 8 últimos bytes por un contador creciente,
    /// en el orden en que SQL Server compara los <c>uniqueidentifier</c>.
    /// </summary>
    /// <param name="input">Guid de partida.</param>
    /// <returns>Guid secuencial.</returns>
    internal static Guid ToSecuencial(this Guid input)
    {
        var counterBytes = new byte[8];

        BinaryPrimitives.WriteInt64LittleEndian(counterBytes, NextSequentialCounter());

        return input.WithCounter(counterBytes);
    }

    /// <summary>
    /// Pone los bytes del contador (en orden little-endian) en los 8 últimos bytes del Guid.
    /// </summary>
    /// <param name="input">Guid de partida.</param>
    /// <param name="counterBytes">Bytes del contador, el menos significativo primero.</param>
    /// <returns>Guid con el contador.</returns>
    internal static Guid WithCounter(this Guid input, byte[] counterBytes)
    {
        var bytes = input.ToByteArray();

        bytes[08] = counterBytes[1];
        bytes[09] = counterBytes[0];
        bytes[10] = counterBytes[7];
        bytes[11] = counterBytes[6];
        bytes[12] = counterBytes[5];
        bytes[13] = counterBytes[4];
        bytes[14] = counterBytes[3];
        bytes[15] = counterBytes[2];

        return new Guid(bytes);
    }

    /// <summary>
    /// Siguiente valor del contador: los ticks actuales o, si no han avanzado, el anterior más uno.
    /// </summary>
    /// <returns>Valor del contador, siempre mayor que el anterior.</returns>
    internal static long NextSequentialCounter()
    {
        long last, next;

        do
        {
            last = Interlocked.Read(ref sequentialCounter);
            next = Math.Max(DateTime.UtcNow.Ticks, last + 1);
        }
        while (Interlocked.CompareExchange(ref sequentialCounter, next, last) != last);

        return next;
    }

    private static void ThrowIfLess(long argument, long min, string? argumentName)
    {
        if (argument < min)
        {
            throw new ArgumentOutOfRangeException(argumentName, Messages.Less(min, argumentName));
        }
    }

    private static string ToThrowIfNotMatch([NotNull] this string? argument, string regex, string? argumentName)
    {
        if (argument is null)
        {
            throw new ArgumentNullException(argumentName, Messages.InvalidFormat(argumentName));
        }

        if (!argument.ToIsRegexMatch(regex))
        {
            throw new ArgumentException(Messages.InvalidFormat(argumentName), argumentName);
        }

        return argument;
    }

    /// <summary>
    /// Mensajes de Shared 2.0.1 (recurso en español), con nombre de campo: el nombre siempre llega
    /// (CallerArgumentExpression), así que no hacen falta las variantes sin nombre.
    /// </summary>
    internal static class Messages
    {
        /// <summary>
        /// Mensaje de campo requerido.
        /// </summary>
        /// <param name="fieldName">Nombre del campo.</param>
        /// <returns>El mensaje.</returns>
        internal static string Required(string? fieldName) => Format("El campo {0} es requerido", fieldName);

        /// <summary>
        /// Mensaje de formato inválido.
        /// </summary>
        /// <param name="fieldName">Nombre del campo.</param>
        /// <returns>El mensaje.</returns>
        internal static string InvalidFormat(string? fieldName) => Format("El campo {0} tiene un formato inválido", fieldName);

        /// <summary>
        /// Mensaje de número menor que el mínimo.
        /// </summary>
        /// <param name="min">Mínimo permitido.</param>
        /// <param name="fieldName">Nombre del campo.</param>
        /// <returns>El mensaje.</returns>
        internal static string Less(long min, string? fieldName) => Format("El campo {0} es menor que {1}", fieldName, min);

        /// <summary>
        /// Mensaje de número fuera de rango.
        /// </summary>
        /// <param name="min">Mínimo permitido.</param>
        /// <param name="max">Máximo permitido.</param>
        /// <param name="fieldName">Nombre del campo.</param>
        /// <returns>El mensaje.</returns>
        internal static string OutRange(long min, long max, string? fieldName) => Format("El campo {0} es menor que {1} o mayor que {2}", fieldName, min, max);

        private static string Format(string format, params object?[] args) => string.Format(CultureInfo.CurrentCulture, format, args);
    }
}
