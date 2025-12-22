namespace SharedScriptsApi.DataModels
{
    public abstract class EntityBase
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public abstract int GetId();
        /// <summary>
        /// Gets the primary key of the entity as an array of objects.
        /// 
        public abstract object[] GetPrimaryKey();
        /// <summary>
        /// Gets or sets the creation date of the entity.
        /// </summary>
        public DateTime ModifiedDate { get; set; }
        /// <summary>
        /// Gets or sets the last modified date of the entity.
        /// </summary>
        public int? ModifiedBy { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the entity is active.
        /// </summary>
        public bool Active { get; set; }
        public EntityBase() { }

    }
}
