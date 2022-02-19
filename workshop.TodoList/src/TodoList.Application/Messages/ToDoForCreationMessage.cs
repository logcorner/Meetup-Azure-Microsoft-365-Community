namespace TodoList.Application.Messages
{
    public class ToDoForCreationMessage
    {
        public readonly string Title;
        public readonly string Description;

        public readonly string ImageUrl;

        public ToDoForCreationMessage(string title, string description, string imageUrl)
        {
            Title = title;
            Description = description;
            ImageUrl = imageUrl;
        }
    }
}