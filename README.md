### Problem

When run with NullGuard the record below named `FooRecordWithInit` returns a `System.InvalidProgramException: 'Common Language Runtime detected an invalid program.'` .
With the setup for NullGuard `Mode="NullableReferenceTypes"` and `IncludeDebugAssert="false"/>`.

```csharp
public record FooRecordWithInit
{
    private int _id;
    public int Id
    {
        init => _id = value;  //< ERROR: Common Language Runtime detected an invalid program.
    }
}
```
The underlying issue seems to be the invalid IL weaved by Fody as seen here:
```csharp
public int Id
{
  init
  {
    _id = value;
    if ((int)/*Error near IL_0007: Stack underflow*/ == 0)
    {
    throw new InvalidOperationException("[NullGuard] Return value of method 'System.Void modreq(System.Runtime.CompilerServices.IsExternalInit) Bug.FooRecordWithInit::set_Id(System.Int32)' is null.");
    }
  }
}
```
In other situations a record works fine with a setter instead of init:
```csharp
public record FooRecordWithSet
{
    private int _id;

    public int Id
    {
        set => _id = value; //< Works
    }
}
```
### Reproduce the issue
A minimal Solution to reproduce the issue can be found [here](https://github.com/sinantutan/nullguard-fody-records-init-bug).

1. Clone the repository.
2. Build the project.
3. Use [ILSpy](https://marketplace.visualstudio.com/items?itemName=SharpDevelopTeam.ILSpy) to view the output. 

### Configuration

Operating System: Windows 11 Enterprise 64-Bit (10.0, Build 22621)
Framework Version: .NET 6 
Packages: Fody 6.6.4, NullGuard.Fody 3.1.0
x64


