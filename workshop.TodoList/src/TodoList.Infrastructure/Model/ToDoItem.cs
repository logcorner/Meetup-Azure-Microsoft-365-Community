namespace TodoList.Infrastructure.Model
{
    public partial class ToDoItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string AssignedTo { get; set; }
        public int TodoId { get; set; }

        public virtual ToDo Todo { get; set; }
    }
}