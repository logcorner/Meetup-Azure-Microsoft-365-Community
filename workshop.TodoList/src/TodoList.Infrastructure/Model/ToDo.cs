using System.Collections.Generic;

namespace TodoList.Infrastructure.Model
{
    public partial class ToDo
    {
        public ToDo()
        {
            ToDoItem = new HashSet<ToDoItem>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string ImageUrl { get; set; }

        public virtual ICollection<ToDoItem> ToDoItem { get; set; }
    }
}