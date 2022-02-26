namespace TodoList.Domain
{
    public sealed class Status
    {
        private readonly StatusEnum _statusEnum;

        private Status(StatusEnum statusEnum)
        {
            _statusEnum = statusEnum;
        }

        public static Status Done = new Status(StatusEnum.Done);
        public static Status InProgress = new Status(StatusEnum.InProgress);
        public static Status Removed = new Status(StatusEnum.Removed);
        public static Status New = new Status(StatusEnum.New);

        public int GetIntValue()
        {
            return (int)_statusEnum;
        }

        public string GetStringValue()
        {
            return _statusEnum.ToString();
        }
    }
}