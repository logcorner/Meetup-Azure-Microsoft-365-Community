using System.ComponentModel.DataAnnotations;

namespace TodoList.WebApi.Models.Todo
{
    public class PaginationParameterDto
    {
        [Required]
        [Range(1, 50, ErrorMessage = "PageNumber should be number between 1 and 50.")]
        public int PageNumber { get; set; }

        [Range(1, 20, ErrorMessage = "PageSize should be number between 1 and 20.")]
        public int PageSize { get; set; }
    }
}