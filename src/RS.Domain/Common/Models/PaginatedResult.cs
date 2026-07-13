



public class PaginatedResult<T>
{
    public IReadOnlyList<T> Data { get; }

    public PaginationMetadata Meta { get; }

    public PaginatedResult(
        IReadOnlyList<T> data,
        PaginationMetadata meta)
    {
        Data = data;
        Meta = meta;
    }
}
