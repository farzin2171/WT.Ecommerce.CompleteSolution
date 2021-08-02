namespace WT.Ecommerce.Domain.Exceptions
{
    public class EntityNotFoundException: CustomException
    {
        private const string MessageTemplate = "Could not find {0} with id '{1}'";

        public string EntityName { get; }

        public string EntityId { get; }

        public EntityNotFoundException(string entityName, string entityId, string detail = null)
            : base(string.Format(MessageTemplate, entityName, entityId), detail)
        {
            EntityName = entityName;
            EntityId = entityId;
        }

    }
}
