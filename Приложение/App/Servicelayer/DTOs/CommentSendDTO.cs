namespace ServiceLayer.Dtos
{
    public class CommentSendDTO
    {
        public int EmployeeId { get; set; }

        public string Comment { get; set; } = null!;
    }
}
