using System.ComponentModel.DataAnnotations;

namespace TodoList.WebApi.Models.Todo
{
    /// <summary>
    /// Model for  new todo creation
    /// </summary>
    public class ToDoForCreationDto
    {
        /// <summary>
        /// The Title
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// The Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The image url
        /// </summary>
        public string ImageUrl { get; set; }
    }
}