
namespace Common.Model
{
    public class GroupUserMessage
    {
        public string Id { get; set; }
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupUserID { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
