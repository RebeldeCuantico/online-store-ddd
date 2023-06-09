﻿using Catalog.Domain.DomainEvents;
using Common.Domain;

namespace Catalog.Domain
{
    public class Category : AggregateRoot
    {
        private Category() : base() { }

        public Category(Name name, Description description, EntityId id) 
            : base(id)
        {
            Name = name;
            Description = description;

            QueueEvent(new CategoryAdded(this));
        }

        public Name Name { get; private set; }

        public Description Description { get; private set; }

        public void Remove()
        {
            QueueEvent(new CategoryRemoved(Id.Value, Name.Value, Description.Value));
        }

        public void ChangeName(string name)
        {
            var oldName = Name.Value;
            Name = new Name(name);

            QueueEvent(new CategoryModified(Id.Value, oldName, name, CategoryFieldName.CategoryName));
        }

        public void ChangeDescription(string description)
        {
            var oldDescription = Description.Value;
            Description = new Description(description);

            QueueEvent(new CategoryModified(Id.Value, oldDescription, description, CategoryFieldName.CategoryDescription));
        }
    }
}
