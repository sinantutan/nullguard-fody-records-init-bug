namespace Bug;

public record FooRecordWithInit
{
    private int _id;

    public int Id
    {
        init => _id = value; //< ERROR: Common Language Runtime detected an invalid program.
    }
}

public record FooRecordWithSet
{
    private int _id;

    public int Id
    {
        set => _id = value; //< Works
    }
}

public class FooClassWithInit
{
    private int _id;

    public int Id
    {
        init => _id = value; //< Works
    }
}