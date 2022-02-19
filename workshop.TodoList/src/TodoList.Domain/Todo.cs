using System;
using System.Collections.Generic;

namespace TodoList.Domain
{
    public class Todo
    {
        public int Id { get; }
        public string Title { get; private set; }
        public string Description { get; private set; }

        private readonly Status _status;
        public Status Status => _status;
        public string ImageUrl { get; private set; }

        private readonly List<TodoItem> _todoItems = new List<TodoItem>();

        public IReadOnlyCollection<TodoItem> Tasks => _todoItems;

        public Todo(int id, string title, string description, string imageUrl)
        {
            Id = id;

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new Exceptions.ArgumentNullException(nameof(title));
            }

            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                var isurl = Uri.TryCreate(imageUrl, UriKind.Absolute, out Uri uriResult) &&
                            (uriResult.Scheme == Uri.UriSchemeHttp
                             || uriResult.Scheme == Uri.UriSchemeHttps
                             || uriResult.Scheme == Uri.UriSchemeFile);
                if (!isurl)
                {
                    throw new Exceptions.ArgumentNullException(nameof(imageUrl));
                }
            }

            Id = id;
            Title = title;
            Description = description;
            _status = Status.New;
            ImageUrl = imageUrl;
        }

        public Todo(string title, string description, string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new Exceptions.ArgumentNullException($"title cannot bu null or empty {nameof(title)}");
            }

            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                var isurl = Uri.TryCreate(imageUrl, UriKind.Absolute, out Uri uriResult) &&
                            (uriResult.Scheme == Uri.UriSchemeHttp
                             || uriResult.Scheme == Uri.UriSchemeHttps
                             || uriResult.Scheme == Uri.UriSchemeFile);
                if (!isurl)
                {
                    throw new Exceptions.ArgumentNullException($"imageUrl is not valid {nameof(imageUrl)}");
                }
            }

            Title = title;
            Description = description;
            _status = Status.New;
            ImageUrl = imageUrl;
        }

        public void AddTasks(List<TodoItem> todoItems)
        {
            if (todoItems == null)
            {
                throw new Exceptions.ArgumentNullException(nameof(todoItems));
            }
            _todoItems.AddRange(todoItems);
        }

        public void RemoveTasks(List<TodoItem> todoItems)
        {
            if (todoItems == null)
            {
                throw new Exceptions.ArgumentNullException(nameof(todoItems));
            }

            foreach (var item in todoItems)
            {
                if (_todoItems.Contains(item))
                {
                    _todoItems.Remove(item);
                }
            }
        }

        public void Update(string title, string description, string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(description) && string.IsNullOrWhiteSpace(imageUrl))
            {
                throw new Exceptions.ArgumentNullException($"{nameof(title)}, {nameof(description)}, {nameof(imageUrl)}");
            }
            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                var isurl = Uri.TryCreate(imageUrl, UriKind.Absolute, out Uri uriResult) &&
                            (uriResult.Scheme == Uri.UriSchemeHttp
                             || uriResult.Scheme == Uri.UriSchemeHttps
                             || uriResult.Scheme == Uri.UriSchemeFile);
                if (!isurl)
                {
                    throw new Exceptions.ArgumentNullException(nameof(imageUrl));
                }
            }

            if (!string.IsNullOrWhiteSpace(title))
            {
                Title = title;
            }
            if (!string.IsNullOrWhiteSpace(description))
            {
                Description = description;
            }
            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                ImageUrl = imageUrl;
            }
        }
    }
}