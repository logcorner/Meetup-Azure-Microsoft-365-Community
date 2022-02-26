using System;

namespace TodoList.Domain
{
    public class TodoItem
    {
        public int Id { get; }
        public string Title { get; }
        public string Description { get; }
        public Status Status { get; }
        public string AssignedTo { get; }

        public TodoItem(int id, string title, string description, Status status, string assignedTo)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentNullException(nameof(title));
            }
            Id = id;
            Title = title;
            Description = description;
            Status = status;
            AssignedTo = assignedTo;
        }
    }
}