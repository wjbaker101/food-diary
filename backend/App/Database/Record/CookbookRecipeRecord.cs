using FluentNHibernate.Mapping;
using System;
using WJBCommon.Lib.Data.Type;

namespace FoodDiaryApi.App.Database.Record
{
    public class CookbookRecipeRecord: DatabaseRecord
    {
        public virtual Guid Reference { get; set; }
        public virtual string Title { get; set; }
        public virtual string Url { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual string WebsiteName { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime AddedAt { get; set; }
        public virtual string Alias { get; set; }
    }

    public sealed class CookbookRecipeRecordMap : ClassMap<CookbookRecipeRecord>
    {
        public CookbookRecipeRecordMap()
        {
            Schema("cookbook");
            Table("recipe");
            Id(x => x.Id, "id").GeneratedBy.Sequence("recipe_id_seq");
            Map(x => x.Reference, "reference");
            Map(x => x.Title, "title");
            Map(x => x.Url, "url");
            Map(x => x.ImageUrl, "image_url");
            Map(x => x.WebsiteName, "website_name");
            Map(x => x.Description, "description");
            Map(x => x.AddedAt, "added_at");
            Map(x => x.Alias, "alias");
        }
    }
}