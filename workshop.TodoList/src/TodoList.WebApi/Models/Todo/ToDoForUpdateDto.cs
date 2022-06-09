namespace TodoList.WebApi.Models.Todo
{
    public class ToDoForUpdateDto
    {
        /// <summary>
        /// The Identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The Image Url
        /// </summary>
        public string ImageUrl { get; set; }
    }
}