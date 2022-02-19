namespace TodoList.Application.Messages
{
    public class ToDoForUpdateMessage
    {
        public readonly int Id;
        public readonly string Title;
        public readonly string Description;

        public readonly string ImageUrl;

        public ToDoForUpdateMessage(int id, string title, string description, string imageUrl)
        {
            Id = id;
            Title = title;
            Description = description;
            ImageUrl = imageUrl;
        }
    }
}