using System.Collections.Generic;
using TodoList.Domain;

namespace TodoList.WebApi.Models.Todo
{
    /// <summary>
    /// Model for Todo List
    /// </summary>
    public class ToDoInListDto
    {
        /// <summary>
        /// The Identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The Status
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// The ImageUrl
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// The list of Tasks
        /// </summary>
        public List<TodoItemInListDto> Tasks { get; set; }
    }
}