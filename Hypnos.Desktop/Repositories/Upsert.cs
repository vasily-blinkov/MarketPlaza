namespace Wholesale.Desktop.Repositories
{
    /// <returns>ID of the updated or the inserted database table record.</returns>
    public delegate short? Upsert(string json);
}
