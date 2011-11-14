using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using TJ.Extensions;

namespace NBlog.Domain.Entities
{
    [Serializable]
    public class Post : Entity
    {
        public string ShortUrl { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public bool Publish { get; set; }
        public DateTime? PublishDate { get; set; }
        public IList<string> Tags { get; set; }
        public IList<string> Categories { get; set; }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Post)) return false;
            return Equals((Post) obj);
        }

        public bool Equals(Post other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            var equal = AreListsEqual(Tags, other.Tags);
            equal = equal && AreListsEqual(Categories, other.Categories);
            return equal && Equals(other.ShortUrl, ShortUrl) && Equals(other.Content, Content) && Equals(other.Title, Title) &&
                   other.Publish.Equals(Publish) && other.PublishDate.Equals(PublishDate);
        }

        private bool AreListsEqual<T>(IList<T> list, IList<T> listToCompare)
        {
            if (list.IsNull())
            {
                if (listToCompare.IsNull())
                {
                    return true;
                }
                return false;
            }
            if (listToCompare.IsNull())
            {
                return false;
            }
            var expectedCount = list.Count;
            var areEqual = expectedCount == listToCompare.Count;
            areEqual = areEqual && list.Where(y => listToCompare.Contains(y)).Count() == expectedCount;
            return areEqual;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (ShortUrl != null ? ShortUrl.GetHashCode() : 0);
                result = (result*397) ^ (Content != null ? Content.GetHashCode() : 0);
                result = (result*397) ^ (Title != null ? Title.GetHashCode() : 0);
                result = (result*397) ^ Publish.GetHashCode();
                result = (result*397) ^ (PublishDate.HasValue ? PublishDate.Value.GetHashCode() : 0);
                result = (result*397) ^ (Tags != null ? Tags.GetHashCode() : 0);
                result = (result*397) ^ (Categories != null ? Categories.GetHashCode() : 0);
                return result;
            }
        }
    }
}
