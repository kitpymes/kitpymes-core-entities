# <img src="https://github.com/kitpymes/kitpymes-core-entities/raw/master/docs/images/logo.png" height="30px"> Kitpymes.Core.Entities

**Entidades base y de sesión, ValueObjects, Shadow Properties y Enumeraciones estaticas**
 
[![Build Status](https://github.com/kitpymes/kitpymes-core-entities/actions/workflows/ci.yml/badge.svg)](https://github.com/kitpymes/kitpymes-core-entities/actions/workflows/ci.yml)
[![NuGet Status](https://img.shields.io/nuget/v/Kitpymes.Core.Entities)](https://www.nuget.org/packages/Kitpymes.Core.Entities/)
[![NuGet Download](https://img.shields.io/nuget/dt/Kitpymes.Core.Entities)](https://www.nuget.org/stats/packages/Kitpymes.Core.Entities?groupby=Version)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/kitpymes/kitpymes-core-entities/blob/master/docs/LICENSE.txt)
[![Size Repo](https://img.shields.io/github/repo-size/kitpymes/kitpymes-core-entities)](https://github.com/kitpymes/kitpymes-core-entities/)
[![Last Commit](https://img.shields.io/github/last-commit/kitpymes/kitpymes-core-entities)](https://github.com/kitpymes/kitpymes-core-entities/)

## 📋 Requerimientos 

* Visual Studio 2026 o superior

* NET TargetFramework = net10.0

* SDK de .NET = 10.0.x

* C# = 12

* Conocimientos sobre Inyección de Dependencias

* Sin dependencias de ASP.NET Core ni de Kitpymes.Core.Shared (desde la 3.0.0): se puede usar en proyectos de dominio.

## 🔧 Instalación 

_Se puede instalar usando el administrador de paquetes Nuget o CLI dotnet._

_Nuget_

```
Install-Package Kitpymes.Core.Entities
```

_CLI dotnet_

```
dotnet add package Kitpymes.Core.Entities
```

## ⌨️ Código

### Entities

```cs
public interface IEntityBase
{
    IReadOnlyCollection<INotification> DomainEvents { get; }

    void AddDomainEvent(INotification domainEvent);

    void RemoveDomainEvent(INotification domainEvent);

    void ClearDomainEvents();
}
```

```cs
public abstract class EntityBase<TKey> : IEquatable<EntityBase<TKey>>, IEntityBase
{
    protected EntityBase() { }

    protected EntityBase(TKey key) {}

    public virtual TKey Id { get; protected set; }
}
```

- **Igualdad:** dos entidades son iguales si son del mismo tipo y tienen la misma clave. Con tipos distintos, `Equals` devuelve `false`. Una entidad sin clave (valor por defecto de `TKey`, p. ej. todavía sin guardar) sólo es igual a sí misma.
- **Eventos de dominio:** son `INotification` de [Kitpymes.Core.Mediator.Abstractions](https://www.nuget.org/packages/Kitpymes.Core.Mediator.Abstractions/). Se publican con `IPublisher.PublishAsync`, en el orden en que se añadieron, y después se vacían con `ClearDomainEvents`.
- `DomainEvents` lleva `[NotMapped]`: no es una columna.

```cs
public sealed class PedidoCreado(Guid pedidoId) : INotification
{
    public Guid PedidoId { get; } = pedidoId;
}

public sealed class Pedido : EntityBaseGuid
{
    public static Pedido Crear()
    {
        var pedido = new Pedido();
        pedido.AddDomainEvent(new PedidoCreado(pedido.Id));
        return pedido;
    }
}

// Al guardar, en la misma transacción:
foreach (var evento in pedido.DomainEvents)
{
    var resultado = await publisher.PublishAsync(evento, cancellationToken);
    if (resultado.IsFailure)
    {
        return resultado;
    }
}

pedido.ClearDomainEvents();
```

```cs
public abstract class EntityBaseGuid : EntityBase<Guid>
{
    protected EntityBaseGuid()
        : base(Guid.NewGuid()) { }
}
```

```cs
public abstract class EntityBaseGuidSequencial : EntityBase<Guid>
{
    protected EntityBaseGuidSequencial()
        : base(Guid.NewGuid().ToSecuencial()) { }
}
```

```cs
public abstract class EntityBaseInt : EntityBase<int>
{
    protected EntityBaseInt() { }

    protected EntityBaseInt(int id)
        : base(id) { }
}
```

```cs
public abstract class EntityBaseLong : EntityBase<long>
{
    protected EntityBaseLong() { }

    protected EntityBaseLong(long id)
        : base(id) { }
}
```

```cs
public abstract class EntityBaseString : EntityBase<string>
{
    protected EntityBaseString() {}

    protected EntityBaseString(string? id) {}
}
```


### Enumerations

```cs
public abstract class EnumerationBase : IComparable
{
    protected EnumerationBase(int id, string name, string? shortName = null) {}

    public int Id { get; private set; }

    public string Name { get; private set; }

    public string? ShortName { get; }

    public static T? ToEnum<T>(int id) {}

    public static T? ToEnum<T>(string? nameOrShortName) {}

    public static T? ToEnum<T>(EnumerationBase enumerationBase) {}

    public static IEnumerable<T> ToAll<T>()
        where T : EnumerationBase {}

    public override string ToString() => Name;

    public string ToFullString() => $"{Id}: {Name}";
}
```

```cs
public class CardEnum : EnumerationBase
{
    public static readonly CardEnum None = new (0, nameof(None), nameof(None));

    public static readonly CardEnum Amex = new (1, "American Express", nameof(Amex));

    public static readonly CardEnum Visa = new (2, nameof(Visa), nameof(Visa));

    public static readonly CardEnum MasterCard = new (3, "Master Card", nameof(MasterCard));

    public CardEnum(int id, string name, string shortName)
        : base(id, name, shortName) {}

    public bool IsNone => Name == None.Name;

    public bool IsAmex => Name == Amex.Name;

    public bool IsVisa => Name == Visa.Name;

    public bool IsMasterCard => Name == MasterCard.Name;
}
```

```cs
public class GenderEnum : EnumerationBase
{
    public static readonly GenderEnum None = new (0, nameof(None), nameof(None));

    public static readonly GenderEnum Male = new (1, nameof(Male), "M");

    public static readonly GenderEnum Female = new (2, nameof(Female), "F");

    public GenderEnum(int id, string name, string shortName)
        : base(id, name, shortName) {}

    public bool IsNone => Name == None.Name;

    public bool IsMale => Name == Male.Name;

    public bool IsFemale => Name == Female.Name;
}
```

```cs
public class LogEnum : EnumerationBase
{
    public static readonly LogEnum None = new (0, nameof(None), nameof(None));

    public static readonly LogEnum Created = new (1, nameof(Created), nameof(Created));

    public static readonly LogEnum Updated = new (2, nameof(Updated), nameof(Updated));

    public static readonly LogEnum Deleted = new (3, nameof(Deleted), nameof(Deleted));

    public static readonly LogEnum Changed = new (4, nameof(Changed), nameof(Changed));

    public LogEnum(int id, string name, string shortName)
        : base(id, name, shortName) {}

    public bool IsNone => Name == None.Name;

    public bool IsCreated => Name == Created.Name;

    public bool IsUpdated => Name == Updated.Name;

    public bool IsDeleted => Name == Deleted.Name;

    public bool IsChanged => Name == Changed.Name;
}
```

```cs
public class StatusEnum : EnumerationBase
{
    public static readonly StatusEnum Inactive = new (0, nameof(Inactive));

    public static readonly StatusEnum Active = new (1, nameof(Active));

    public StatusEnum(int id, string name)
        : base(id, name) {}

    public bool IsActive => Name == Active.Name;

    public bool IsInactive => Name == Inactive.Name;

    public static StatusEnum ToEnum(bool status) => status ? Active : Inactive;

    public static StatusEnum ToEnum(string? status) {}

    public static bool ToBool(StatusEnum status) => status.ToThrowIfNullOrEmpty().Name == Active.ToString();

    public static bool ToBool(string? status) {}
}
```

```cs
public class SubscriptionEnum : EnumerationBase
{
    public static readonly SubscriptionEnum None = new (0, nameof(None), nameof(None));

    public static readonly SubscriptionEnum Free = new (1, nameof(Free), nameof(Free));

    public static readonly SubscriptionEnum Silver = new (2, nameof(Silver), nameof(Silver));

    public static readonly SubscriptionEnum Gold = new (3, nameof(Gold), nameof(Gold));

    public SubscriptionEnum(int id, string name, string shortName)
        : base(id, name, shortName) {}

    public bool IsNone => Name == None.Name;

    public bool IsFree => Name == Free.Name;

    public bool IsSilver => Name == Silver.Name;

    public bool IsGold => Name == Gold.Name;
}
```

### ShadowProperties

```cs
public interface IActive
{
    public const string IsActive = nameof(IsActive);
}
```

```cs
public interface IAudited : IAuditedCreation, IAuditedModification {}
```

```cs
public interface IAuditedCreation
{
    public const string CreatedDate = nameof(CreatedDate);

    public const string CreatedUserId = nameof(CreatedUserId);
}
```

```cs
public interface IAuditedDeletion
{
    public const string DeletedDate = nameof(DeletedDate);

    public const string DeletedUserId = nameof(DeletedUserId);
}
```

```cs
public interface IAuditedFull : IAuditedCreation, IAuditedModification, IAuditedDeletion {}
```

```cs
public interface IAuditedModification
{
    public const string ModifiedDate = nameof(ModifiedDate);

    public const string ModifiedUserId = nameof(ModifiedUserId);
}
```

```cs
public interface IDefault
{
    public const string IsDefault = nameof(IsDefault);
}
```

```cs
public interface IDelete
{
    public const string IsDelete = nameof(IsDelete);
}
```

```cs
public interface INotMapped
{
    public const string IsNotMapped = nameof(IsNotMapped);
}
```

```cs
public interface INotVisible
{
    public const string IsNotVisible = nameof(IsNotVisible);
}
```

```cs
public interface IRowVersion
{
    public const string RowVersion = nameof(RowVersion);
}
```

```cs
public interface IStatic
{
    public const string IsStatic = nameof(IsStatic);
}
```

```cs
public interface ITenant
{
    public const string TenantId = nameof(TenantId);
}
```

### ValueObjects

```cs
public abstract class ValueObjectBase : INotMapped
{    
}
```

```cs
public sealed class Address : ValueObjectBase
{
    public static Address Null { get;}

    public bool IsNull { get;}

    public string? Street { get; }

    public int? Number { get; }

    public string? PostalCode { get; }

    public string? City { get; }

    public string? State { get; }

    public string? Country { get; }

    [return: NotNull]
    public static Address Create(string street, int number, string postalCode, string city, string state, string country) {}

    [return: NotNull]
    public Address ChangeStreet(string? street) {}

    [return: NotNull]
    public Address ChangeNumber(int number) {}

    [return: NotNull]
    public Address ChangePostalCode(string? postalCode) {}

    [return: NotNull]
    public Address ChangeCity(string? city) {}

    [return: NotNull]
    public Address ChangeState(string? state) {}

    [return: NotNull]
    public Address ChangeCountry(string? country) {}

    public override string ToString() {}
}
```

```cs
public sealed class Currency : ValueObjectBase
{
    public enum CodeName
    {
        AED = 784,
        AFN = 971,
        ALL = 008,
        AMD = 051,
        ANG = 532,
        AOA = 973,
        ARS = 032,
        AUD = 036,
        AWG = 533,
        AZN = 944,
        BAM = 977,
        BBD = 052,
        BDT = 050,
        BGN = 975,
        BHD = 048,
        BIF = 108,
        BMD = 060,
        BND = 096,
        BOB = 068,
        BOV = 984,
        BRL = 986,
        BSD = 044,
        BTN = 064,
        BWP = 072,
        BYR = 974,
        BZD = 084,
        CAD = 124,
        CDF = 976,
        CHE = 947,
        CHF = 756,
        CHW = 948,
        CLF = 990,
        CLP = 152,
        CNY = 156,
        COP = 170,
        COU = 970,
        CRC = 188,
        CUC = 931,
        CUP = 192,
        CVE = 132,
        CZK = 203,
        DJF = 262,
        DKK = 208,
        DOP = 214,
        DZD = 012,
        EGP = 818,
        ERN = 232,
        ETB = 230,
        EUR = 978,
        FJD = 242,
        FKP = 238,
        GBP = 826,
        GEL = 981,
        GHS = 936,
        GIP = 292,
        GMD = 270,
        GNF = 324,
        GTQ = 320,
        GYD = 328,
        HKD = 344,
        HNL = 340,
        HRK = 191,
        HTG = 332,
        HUF = 348,
        IDR = 360,
        ILS = 376,
        INR = 356,
        IQD = 368,
        IRR = 364,
        ISK = 352,
        JMD = 388,
        JOD = 400,
        JPY = 392,
        KES = 404,
        KGS = 417,
        KHR = 116,
        KMF = 174,
        KPW = 408,
        KRW = 410,
        KWD = 414,
        KYD = 136,
        KZT = 398,
        LAK = 418,
        LBP = 422,
        LKR = 144,
        LRD = 430,
        LSL = 426,
        LYD = 434,
        MAD = 504,
        MDL = 498,
        MGA = 969,
        MKD = 807,
        MMK = 104,
        MNT = 496,
        MOP = 446,
        MRO = 478,
        MUR = 480,
        MVR = 462,
        MWK = 454,
        MXN = 484,
        MXV = 979,
        MYR = 458,
        MZN = 943,
        NAD = 516,
        NGN = 566,
        NIO = 558,
        NOK = 578,
        NPR = 524,
        NZD = 554,
        OMR = 512,
        PAB = 590,
        PEN = 604,
        PGK = 598,
        PHP = 608,
        PKR = 586,
        PLN = 985,
        PYG = 600,
        QAR = 634,
        RON = 946,
        RSD = 941,
        RUB = 643,
        RWF = 646,
        SAR = 682,
        SBD = 090,
        SCR = 690,
        SDG = 938,
        SEK = 752,
        SGD = 702,
        SHP = 654,
        SLL = 694,
        SOS = 706,
        SRD = 968,
        SSP = 728,
        STD = 678,
        SVC = 222,
        SYP = 760,
        SZL = 748,
        THB = 764,
        TJS = 972,
        TMT = 934,
        TND = 788,
        TOP = 776,
        TRY = 949,
        TTD = 780,
        TWD = 901,
        TZS = 834,
        UAH = 980,
        UGX = 800,
        USD = 840,
        USN = 997,
        UYI = 940,
        UYU = 858,
        UZS = 860,
        VEF = 937,
        VND = 704,
        VUV = 548,
        WST = 882,
        XAF = 950,
        XAG = 961,
        XAU = 959,
        XBA = 955,
        XBB = 956,
        XBC = 957,
        XBD = 958,
        XCD = 951,
        XDR = 960,
        XOF = 952,
        XPD = 964,
        XPF = 953,
        XPT = 962,
        XSU = 994,
        XTS = 963,
        XUA = 965,
        XXX = 999,
        YER = 886,
        ZAR = 710,
        ZMW = 967,
        ZWL = 932
    }

    public static Currency Default => new Currency();

    public string? Symbol { get; }

    public string? Code { get; }

    public string? Name { get; }

    [return: NotNull]
    public static Currency Create(CodeName code) => new Currency(code);

    [return: NotNull]
    public Currency ChangeCurrency(CodeName code) {}

    public override string ToString() {}
}
```

```cs
public sealed class Email : ValueObjectBase
{
    public static Email Null => new Email();

    public bool IsNull => Value.ToIsNullOrEmpty();

    public string? Value { get; }

    [return: NotNull]
    public static Email Create(string? email) => new Email(email);

    public void Change(string? email) => Value = email.ToIsEmailThrow(nameof(email));

    public override string ToString() {}

    public string? ToNormalize() {}
}
```

```cs
public sealed class FullName : ValueObjectBase
{
    public static FullName Null => new FullName();

    public bool IsNull => FirstName.ToIsNullOrEmpty();

    public string? FirstName { get; }

    public string? MiddleName { get; }

    public string? LastName { get; }

    [return: NotNull]
    public static FullName Create(string? firstName, string lastName, string? middleName = null) {}

    [return: NotNull]
    public FullName ChangeFirstName(string? firstName) {}

    [return: NotNull]
    public FullName ChangeMiddleName(string? middleName) {}

    [return: NotNull]
    public FullName ChangeLastName(string? lastName) {}

    public override string ToString() {}

    public string ToFullString() {}
}
```

```cs
public sealed class Money : ValueObjectBase
{
    public static Money Default => new Money();

    public decimal Amount { get; }

    public Currency? Currency { get; }

    public int? NumbeOfDecimals { get; }

    [return: NotNull]
    public static Money Create(decimal amount, Currency currency, int numbeOfDecimals = DefaultNumbeOfDecimals) {}

    [return: NotNull]
    public Money ChangeAmount(decimal amount) {}

    [return: NotNull]
    public Money ChangeCurrency(Currency currency) {}

    [return: NotNull]
    public Money ChangeNumbeOfDecimals(int numbeOfDecimals) {}

    public override string ToString() {}
}
```

```cs
public sealed class Name : ValueObjectBase
{
    public static Name Null { get; }

    public bool IsNull { get; }

    public string? Value { get; }

    [return: NotNull]
    public static Name Create(string? name) {}

    public void Change(string? name) {}

    public override string ToString() {}

    public string? ToNormalize() {}
}
```

```cs
public sealed class StringId : ValueObjectBase
{
    public static StringId Null  { get; }

    public bool IsNull  { get; }

    public string? Value { get; }

    [return: NotNull]
    public static StringId Create() {}

    [return: NotNull]
    public static StringId Create(string? id) {}

    public override string ToString() {}
}
```

```cs
public sealed class Subdomain : ValueObjectBase
{
    public static Subdomain Null { get; }

    public bool IsNull { get; }

    public string? Value { get; }

    [return: NotNull]
    public static Subdomain Create(string? subdomain) {}

    public void Change(string? subdomain) {}

    public override string ToString() {}
}
```

```cs
public sealed class Telephone : ValueObjectBase
{
    public static Telephone Null { get; }

    public bool IsNull { get; }

    public string? Prefix { get; }

    public long? Number { get; }

    [return: NotNull]
    public static Telephone Create(string? prefix, long number) {}

    [return: NotNull]
    public Telephone ChangePrefix(string? prefix) {}

    [return: NotNull]
    public Telephone ChangeNumber(long number) {}

    public override string ToString() {}
}
```

```cs
public sealed class TenantName : ValueObjectBase
{
    public static TenantName Null { get; }

    public bool IsNull { get; }

    public string? Value { get; }

    [return: NotNull]
    public static TenantName Create(string? name) {}

    public void Change(string? name) {}

    public override string ToString() {}
}
```

## ⚙️ Pruebas Unitarias

_Cada proyecto tiene su respectivo test, se ejecutan desde el "Explorador de pruebas"_

![Tests](images/screenshot/pruebas_unitarias.png)


## 🛠️ Construido con 

* [NET Core](https://dotnet.microsoft.com/download) - Framework de trabajo
* [C#](https://docs.microsoft.com/es-es/dotnet/csharp/) - Lenguaje de programación
* [Inserción de dependencias](https://docs.microsoft.com/es-es/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.0) - Patrón de diseño de software
* [MSTest](https://docs.microsoft.com/es-es/dotnet/core/testing/unit-testing-with-mstest) - Pruebas unitarias
* [Nuget](https://www.nuget.org/) - Manejador de dependencias
* [Visual Studio](https://visualstudio.microsoft.com/) - Entorno de programacion


## ✒️ Autores 

* **Sebastian R Ferrari** - *Trabajo Inicial* - [kitpymes](https://kitpymes.com)


## 📄 Licencia 

* Este proyecto está bajo la Licencia [LICENSE](LICENSE.txt)


## 🎁 Gratitud 

* Este proyecto fue diseñado para compartir, creemos que es la mejor forma de ayudar 📢
* Cada persona que contribuya sera invitada a tomar una 🍺 
* Gracias a todos! 🤓

---
[Kitpymes](https://github.com/kitpymes) 😊