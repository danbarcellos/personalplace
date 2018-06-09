using System;

namespace PersonalPlace.Application.Api.Services.Controllers.Catalog
{
    public class CommentDTO
    {
        public virtual Guid MentionId { get; protected set; }

        public virtual DateTimeOffset DateTime { get; protected set; }

        public virtual string Text { get; set; }
    }
}