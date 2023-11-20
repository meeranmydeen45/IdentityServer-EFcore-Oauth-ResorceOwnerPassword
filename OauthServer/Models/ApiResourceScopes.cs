namespace OauthServer.Models
{
    public class ApiResourceScopes
    {
        public int Id { get; set; }
        public string Scope { get; set; }
        public int ApiResourceId { get; set; }
    }
}
