# What is Nett
Nett is a library that helps to read and write [TOML](https://github.com/toml-lang/toml) files in .Net.

# Objectives
+ Compatible (but not 100% compliant) with **latest** TOML spec. All currently available TOML libs for C# (.Net) are pre 0.1.0 spec

# Differences to original TOML spec
* Nett also allows you to specify timespan values. Timespan currently isn't supported in the original
TOML spec. The following TimeSpan formats are supported:

+ hh:mm
+ hh:mm:ss
+ hh:mm:ss.ff
+ dd.hh:mm:ss
+ dd.hh:mm:ss.ff

# Getting Started

All common TOML operations are performed via the static class 'Nett.Toml'. Although there are other
types available from the library in general using that single type should be sufficient
for most standard scenarios.

The following example shows how you can write and read some complex object to and from a
TOML file. The object that gets serialized and deserialized is defined as follows:

```C#
public class Configuration
{
    public bool EnableDebug { get; set; }
    public Server Server { get; set; }
    public Client Client { get; set; }
}

public class Server
{
    public TimeSpan Timeout { get; set; }
}

public class Client
{
    public string ServerAddress { get; set; }
}
```

To write the above object to a TOML File you have to do:

```C#
var config = new Configuration()
{
    EnableDebug = true,
    Server = new Server() { Timeout = TimeSpan.FromMinutes(1) },
    Client = new Client() { ServerAddress = "http://127.0.0.1:8080" },
};

Toml.WriteFile(config, "test.tml");
```

This will write the following content to your hard disk:

```
EnableDebug = false

[Server]
Timeout = 00:01:00


[Client]
ServerAddress = "http://127.0.0.1:8080"


```

To read that back into your object you need to:

```C#
var config = Toml.ReadFile<Configuration>("test.tml");
```

If you only have a TOML file but no corresponding class that the data in the TOML file
maps to, you can read the data into a generic TomlTable structure and extract a member
the like:

```C#
TomlTable table = Toml.ReadFile("test.tml");
var timeout = table.Get<TomlTable>("Server").Get<TimeSpan>("Timeout");
```

# Advanced Topics
All advanced concepts of the TOML serialization and deserialization process can be controlled
via C# attributes or providing a custom TomlConfiguration instance when invoking threa Read/Write
methods.

To create a new configuration do the following:
```C#
var myConfig = TomlConfig.Create();
```

This will creat a copy of the default configuration, and you can configure this new copy
via builder methods that will be described in more detail in the next topics.

## Deserializing types without default constructor
If your type doesn't have a default constructor Nett will not be able to deserialize
into that type without some help.

Assume we have the following type, that extends the configuration class from the basic
examples:

```C#
public class ConfigurationWithDepdendency : Configuration
{
    public ConfigurationWithDepdendency(object dependency)
    {

    }
}
```

When you try to deserialze the `test.tml` into that type via

```C#
var config = Toml.ReadFile<ConfigurationWithDepdendency>("test.tml");
```

you will get the following exception:

`Failed to create type 'ConfigurationWithDepdendency'. Only types with a parameterless constructor or an
specialized creator can be created. Make sure the type has a parameterless constructor or a
configuration with an corresponding creator is provided.`

To make this work, we need to pass a custom configuration to the read method that tells Nett, how
the type can be activated. This is done the by:

```C#
var myConfig = TomlConfig.Create()
    .ConfigureType<ConfigurationWithDepdendency>()
        .As.CreateWith(() => new ConfigurationWithDepdendency(new object()))
    .Apply();

var config = Toml.ReadFile<ConfigurationWithDepdendency>("test.tml", myConfig);
```

# Changelog
[TBD] v1.0.0 (compatible with TOML 0.4.0) Initial Release


