namespace VoteAnalyzer.DataAccessLayer.Entities
{
    public interface IIdentifiable<TId>
    {
        TId Id { get; set; }
    }
}
